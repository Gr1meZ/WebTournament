using DataAccess.Abstract;
using DataAccess.Common.Exceptions;
using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;
using static System.String;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebTournament.Business.Services;

public class BracketService : IBracketService
{
    private readonly IApplicationDbContext _appDbContext;
    
    public BracketService(IApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task<Select2Response> GetAutoCompleteBracketFighters(Select2Request request, Guid bracketId)
    {
        var fighters = _appDbContext.Fighters
            .Where(x => x.BracketId == bracketId)
            .AsNoTracking()
            .AsQueryable();

        var dbQuery = fighters;
        var total = await fighters.CountAsync();

        if (!IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.Surname.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname}"
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
    
    public async Task SaveState(BracketState bracketState)
    {
        var bracket = await _appDbContext.Brackets.FindAsync(bracketState.Id);
        if (bracket != null)
        {
            bracket.State = bracketState.State;
            SyncWinners(bracketState);
            await _appDbContext.SaveChangesAsync();
        }
    }
    
    private void SyncWinners(BracketState bracketState)
    {

        if (bracketState.Winners?.Count == null)
            return;

        _appDbContext.BracketWinners.Update(new BracketWinner()
        {
            Id = bracketState.Id,
            FirstPlaceId = bracketState.Winners.ElementAtOrDefault(0) == Guid.Empty ? null : bracketState.Winners[0], 
            SecondPlaceId = bracketState.Winners.ElementAtOrDefault(1) == Guid.Empty ? null : bracketState.Winners[1],
            ThirdPlaceId = bracketState.Winners.ElementAtOrDefault(2) == Guid.Empty ? null : bracketState.Winners[2]
        });
        
        /*_appDbContext.BracketWinners.Update(new BracketWinner
    {
        Id = bracket.Id,
        FirstPlaceId = winners[0].HasValue ? Guid.Parse(winners[0].ToString() ?? Empty) : null,
        SecondPlaceId = winners[1].HasValue ? Guid.Parse(winners[1].ToString() ?? Empty) : null,
        ThirdPlaceId = winners[2].HasValue ? Guid.Parse(winners[2].ToString() ?? Empty) : null
    });
    */
    }
    
    public async Task<BracketState> GetBracket(Guid bracketId)
    {
        var bracket = await _appDbContext.Brackets
            .Include(x => x.WeightCategorie.AgeGroup)
            .FirstOrDefaultAsync(x => x.Id == bracketId);
            
        
        if (bracket == null)
            throw new ValidationException("ValidationException", "Bracket not found");
        
        var bracketViewModel = new BracketState()
        {
            Id = bracket.Id,
            State = bracket.State,
            Winners = new List<Guid>(),
            CategorieName = $"{bracket.WeightCategorie.AgeGroup.Name} - {bracket.WeightCategorie.WeightName} - {Join(", ",  _appDbContext.Belts.OrderBy(belt => belt.BeltNumber)
                .Where(belt => bracket.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}"))}"
        };
        return bracketViewModel;
    }
    public async Task GenerateBrackets(BracketViewModel bracketViewModel)
    {
        if (bracketViewModel?.AgeGroupId == Guid.Empty)
        {
            throw new ValidationException("ValidationException", "Не выбрана возрастная группа!");
        }
        
        var weightCategoriesId =  await _appDbContext.AgeGroups
            .SelectMany(x => x.WeightCategories)
            .Where(x => x.AgeGroupId == bracketViewModel.AgeGroupId)
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

    private async  Task CreateBracketWinners()
    {
        var bracketIds = _appDbContext.Brackets.Select(x => x.Id).ToList();
        if (!bracketIds.Any() || _appDbContext.BracketWinners.Any(x => bracketIds.Contains(x.Id))) return;
        {
            var bracketWinners = bracketIds.Select(x => new BracketWinner
            {
                Id = x
            });

            await _appDbContext.BracketWinners.AddRangeAsync(bracketWinners);
        }
    }
    public async Task DistributeAllPlayers(Guid tournamentId)
    {
        var fighters = await _appDbContext.Tournaments.SelectMany(x => x.Fighters)
            .Include(x => x.WeightCategorie.AgeGroup)
            .Include(x => x.Belt)
            .Include(x => x.Bracket)
            .Include(x => x.Tournament)
            .Include( x => x.Trainer.Club)
            .Where(x => x.TournamentId == tournamentId)
            .ToListAsync();
        
        foreach (var fighter in fighters)
        {
            var bracketId = await _appDbContext.Brackets
                .Where(x => x.WeightCategorieId == fighter.WeightCategorieId && x.Division.Contains(fighter.BeltId) && x.TournamentId == tournamentId)
                .Select(x => x.Id).FirstOrDefaultAsync();

            if (bracketId == Guid.Empty)
                throw new ValidationException("ValidationException", $"Не найдена подходящая сетка для игрока {fighter.Surname} {fighter.Name}. Создайте все необходимые сетки перед жеребьевкой!");

            fighter.BracketId = bracketId;
        }
        
        await DrawFighters(tournamentId, fighters); 
        await CreateBracketWinners();
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
                        f.WeightCategorie.MaxWeight.ToString().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!IsNullOrWhiteSpace(request.OrderColumn) && !IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "divisionName" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Division)
                        : dbQuery.OrderByDescending(o => o.Division),
                    "categoriesName" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.WeightCategorie.AgeGroup.MinAge)
                    : dbQuery.OrderByDescending(o => o.WeightCategorie.AgeGroup.MinAge),
                    "maxWeight" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.WeightCategorie.MaxWeight)
                        : dbQuery.OrderByDescending(o => o.WeightCategorie.MaxWeight),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.WeightCategorie.MaxWeight)
                        : dbQuery.OrderByDescending(o => o.WeightCategorie.MaxWeight)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            var dbItems = await dbQuery.Select(x => new BracketViewModel()
            {
                Id = x.Id,
                DivisionName = Join(", ",  _appDbContext.Belts.OrderBy(belt => belt.BeltNumber)
                    .Where(belt => x.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}")),
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
    
    private async Task DrawFighters(Guid tournamentId, List<Fighter> fighters)
    {
        var brackets =  await _appDbContext.Brackets
            .Where(x => x.TournamentId == tournamentId)
            .ToListAsync();
        
        foreach (var bracket in brackets)
        {
            var bracketData = new BracketData()
            {
                teams = new List<List<Team>>(),
                results = new List<List<List<List<int?>>>>()
            };

            var bracketFighters = fighters.Where(x => x.BracketId == bracket.Id).ToList();

            if (bracketFighters.Count == 0)
            {
                _appDbContext.Brackets.Remove(bracket);
                continue;
            }
            
            while (bracketFighters.Count > 0)
            {
                var firstPlayer = bracketFighters.FirstOrDefault();
                var secondPlayer = bracketFighters.FirstOrDefault(x => x.Country != firstPlayer?.Country
                                                                       || x.City != firstPlayer.City
                                                                       || x.Trainer.ClubId != firstPlayer.Trainer.ClubId
                                                                       || x.TrainerId != firstPlayer.TrainerId) ?? bracketFighters.FirstOrDefault(x => x != firstPlayer);
                
                bracketData.teams.Add(new List<Team>()
                {
                    new() {name = firstPlayer!.Surname, id = firstPlayer.Id.ToString()},
                    (secondPlayer == null ? null : new Team {name = secondPlayer.Surname, id = secondPlayer.Id.ToString()})! 
                });
                
                bracketFighters.Remove(firstPlayer);
                if (secondPlayer != null) bracketFighters.Remove(secondPlayer);
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