using DataAccess.Abstract;
using DataAccess.Common.Exceptions;
using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;
using static System.String;

namespace WebTournament.Business.Services;

public class BracketService : IBracketService
{
    private readonly IApplicationDbContext _appDbContext;
    
    public BracketService(IApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<BracketState> GetBracket(Guid bracketId)
    {
        var bracket = await _appDbContext.Brackets.FindAsync(bracketId);
        
        if (bracket == null)
            throw new ValidationException("ValidationException", "Bracket not found");
        
        var bracketViewModel = new BracketState()
        {
            Id = bracket.Id,
            State = bracket.State
        };
        return bracketViewModel;
    }
    public async Task GenerateBrackets(BracketViewModel bracketViewModel)
    {
        
        var weightCategoriesId =  await _appDbContext.AgeGroups
            .SelectMany(x => x.WeightCategories)
            .Where(x => x.AgeGroupId == bracketViewModel.AgeGroupId && x.Fighters.Any(y => y.WeightCategorieId == x.Id && bracketViewModel.Division.Contains(y.BeltId)))
            .Select(x => x.Id)
            .ToListAsync();
        
        foreach (var categorieId in weightCategoriesId)
        {
            await CheckBrackets(bracketViewModel.TournamentId, categorieId, bracketViewModel.Division);
            await _appDbContext.Brackets.AddAsync(new Bracket()
            {
                Division = bracketViewModel.Division,
                TournamentId = bracketViewModel.TournamentId,
                WeightCategorieId = categorieId,
                State = Empty
            });
        }

        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task DistributeAllPlayers(Guid tournamentId)
    {
        var fighters = await _appDbContext.Tournaments.SelectMany(x => x.Fighters)
            .Include(x => x.WeightCategorie.AgeGroup)
            .Include(x => x.Belt)
            .Include(x => x.Bracket)
            .Include(x => x.Tournament)
            .Where(x => x.TournamentId == tournamentId)
            .ToListAsync();
        
        foreach (var fighter in fighters)
        {
            var bracketId = await _appDbContext.Brackets
                .Where(x => x.WeightCategorieId == fighter.WeightCategorieId && x.Division.Contains(fighter.BeltId) && x.TournamentId == tournamentId)
                .Select(x => x.Id).FirstOrDefaultAsync();

            if (bracketId == Guid.Empty)
                throw new ValidationException("ValidationException", "Не найдена подходящая сетка для игрока");

            fighter.BracketId = bracketId;
        }
        
        await DrawFighters(tournamentId);
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<PagedResponse<BracketViewModel[]>> BracketsList(PagedRequest request, Guid tournamentId)
        {
            var dbQuery = _appDbContext.Brackets
                .Include(x => x.Tournament)
                .Include(x => x.WeightCategorie.AgeGroup)
                .Where(x => x.TournamentId == tournamentId)
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = lowerQ.Split(' ').Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Tournament.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!IsNullOrWhiteSpace(request.OrderColumn) && !IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "tournamentName" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Tournament.Name)
                        : dbQuery.OrderByDescending(o => o.Tournament.Name),
                    "ageGroup" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.WeightCategorie.AgeGroup.Name)
                    : dbQuery.OrderByDescending(o => o.WeightCategorie.AgeGroup.Name),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            var dbItems = await dbQuery.Select(x => new BracketViewModel()
            {
                Id = x.Id,
                DivisionName = Join(", ",  _appDbContext.Belts.Where(belt => x.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}")),
                CategoriesName = $"{x.WeightCategorie.AgeGroup.Name} - {x.WeightCategorie.WeightName}",
                MaxWeight = x.WeightCategorie.MaxWeight
            }).ToArrayAsync();

            return new PagedResponse<BracketViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

    public async Task DeleteBracket(Guid id)
    {
        var bracket = await _appDbContext.Brackets.FindAsync(id) ?? throw new ValidationException("ValidationException", "Bracket not found");
        _appDbContext.Brackets.Remove(bracket);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAllBrackets(Guid tournamentId)
    {
        var brackets = _appDbContext.Brackets
            .Include(x => x.Fighters)
            .Include(x => x.WeightCategorie.AgeGroup)
            .Include(x => x.Tournament)
            .Where(x => x.TournamentId == tournamentId);
        
         _appDbContext.Brackets.RemoveRange(brackets);
         await _appDbContext.SaveChangesAsync();
    }

    private async Task CheckBrackets(Guid tournamentId, Guid weightCategorieId, Guid[] division)
    {
        var bracketsExists = await _appDbContext.Brackets.AnyAsync(x => x.TournamentId == tournamentId && x.WeightCategorieId == weightCategorieId && x.Division == division);
        if (bracketsExists)
            throw new ValidationException("ValidationException", "Данная турнирная сетка уже сгенерирована для данного турнира");
    }
    
    private async Task DrawFighters(Guid tournamentId)
    {
        var brackets =  await _appDbContext.Brackets
            .Include(x => x.Fighters)
            .Include( x => x.WeightCategorie.AgeGroup)
            .Include(x => x.Tournament)
            .Where(x => x.TournamentId == tournamentId)
            .ToListAsync();
        
        foreach (var bracket in brackets)
        {
            var bracketData = new BracketData()
            {
                teams = new List<List<Team>>(),
                results = new List<int>()
            };
            
            var playersFromQuery = bracket.Fighters.Select(x => x.Id).ToList();
            
            var players = await _appDbContext.Fighters.Where(x => playersFromQuery.Contains(x.Id))
                .Include(x => x.WeightCategorie.AgeGroup)
                .Include(x => x.Tournament)
                .Include(x => x.Trainer.Club)
                .ToListAsync();
            
            if (players.Count == 0)
                throw new ValidationException("ValidationException", "Игроки не найдены");
            
            while (players.Count > 0)
            {
                var firstPlayer = players.FirstOrDefault();
                var secondPlayer = players.FirstOrDefault(x => x.Country != firstPlayer?.Country
                                                               || x.City != firstPlayer.City
                                                               || x.Trainer.ClubId != firstPlayer.Trainer.ClubId
                                                               || x.TrainerId != firstPlayer.TrainerId) ?? players.FirstOrDefault(x => x != firstPlayer);
                
                bracketData.teams.Add(new List<Team>()
                {
                    new() {name = firstPlayer!.Surname},
                    (secondPlayer == null ? null : new() {name = secondPlayer!.Surname})! 
                });
                
                players.Remove(firstPlayer);
                players.Remove(secondPlayer);
            }

            ValidateBracketData(bracketData);
            bracket.State = JsonConvert.SerializeObject(bracketData);
        }

    }

    private void ValidateBracketData(BracketData bracketData)
    {
        var count = bracketData.teams.Count;
        var nearestPowerOfTwo = 1;
        
        while (nearestPowerOfTwo < count)
        {
            nearestPowerOfTwo *= 2;
        }

        if (nearestPowerOfTwo <= count) return;
        var numberOfEmptyPlayers = nearestPowerOfTwo - count;
        for (var i = 0; i < numberOfEmptyPlayers; i++)
        {
            bracketData.teams.Add(new List<Team>()
            {
                null!, null!
            });
        }
    }
}