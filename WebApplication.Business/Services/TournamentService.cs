using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DataAccess.Abstract;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;
using static System.String;

namespace WebTournament.Business.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IApplicationDbContext _appDbContext;

        public TournamentService(IApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddTournamentAsync(TournamentViewModel tournamentViewModel)
        {
            if (tournamentViewModel == null)
                throw new ValidationException("Tournament model is null");

            var tournament = new Tournament()
            {
                Address = tournamentViewModel.Address,
                Name = tournamentViewModel.Name,
                StartDate = tournamentViewModel.StartDate,
            };

            _appDbContext.Tournaments.Add(tournament);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteTournamentAsync(Guid id)
        {
            var tournament = await _appDbContext.Tournaments.
                 Include( xx => xx.Fighters)
                .Include(x => x.Brackets)
                .FirstOrDefaultAsync()?? throw new ValidationException("Tournament not found");

            var bracketWinners =  _appDbContext.BracketWinners.Include(x => x.Bracket)
                .Where(x => x.Bracket.TournamentId == id);
            
            _appDbContext.BracketWinners.RemoveRange(bracketWinners);
            _appDbContext.Fighters.RemoveRange(tournament.Fighters);
            _appDbContext.Brackets.RemoveRange(tournament.Brackets);
            
            _appDbContext.Tournaments.Remove(tournament);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditTournamentAsync(TournamentViewModel tournamentViewModel)
        {
            if (tournamentViewModel == null)
                throw new ValidationException("Tournament model is null");

            var tournament = await _appDbContext.Tournaments.FindAsync(tournamentViewModel.Id);


            tournament!.Name = tournamentViewModel.Name;
            tournament.Address = tournamentViewModel.Address;
            tournament.StartDate = tournamentViewModel.StartDate;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TournamentViewModel> GetTournamentAsync(Guid id)
        {
            var tournament = await _appDbContext.Tournaments.FindAsync(id);
            
            if (tournament == null)
                throw new ValidationException("Tournament not found");
            
            var viewModel = new TournamentViewModel()
            {
                Id = tournament.Id,
                StartDate = tournament.StartDate,
                Address = tournament.Address, 
                Name = tournament.Name  
            };

            return viewModel;
        }

        public async Task<List<TournamentViewModel>> GetTournaments()
        {
            var tournaments = _appDbContext.Tournaments.AsNoTracking();

            return await tournaments.Select(x => new TournamentViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                StartDate = x.StartDate
            }).ToListAsync();
        }

        public async Task<PagedResponse<TournamentViewModel[]>> TournamentsListAsync(PagedRequest request)
        {
            var dbQuery = _appDbContext.Tournaments
              .AsQueryable()
              .AsNoTracking();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.StartDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Address.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!IsNullOrWhiteSpace(request.OrderColumn) && !IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Name)
                        : dbQuery.OrderByDescending(o => o.Name),
                    "startDate" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.StartDate)
                    : dbQuery.OrderByDescending(o => o.StartDate),
                    "address" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Address)
                        : dbQuery.OrderByDescending(o => o.Address),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new TournamentViewModel()
            {
                Id = x.Id,
                Address = x.Address,
                Name = x.Name,
                StartDate = x.StartDate
            }).ToArrayAsync();

            return new PagedResponse<TournamentViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task<Select2Response> GetSelect2TournamentsAsync(Select2Request request)
        {
            var tournaments = _appDbContext.Tournaments
              .AsNoTracking()
              .AsQueryable();

            var dbQuery = tournaments;
            var total = await tournaments.CountAsync();

            if (!IsNullOrWhiteSpace(request.Search))
            {
                dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));
            }

            if (request.PageSize != -1)
                dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

            var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
                .ToArray();

            return new Select2Response()
            {
                Data = data,
                Total = total
            };
        }

        public async Task<List<BracketWinnerViewModel>> GetTournamentResultsAsync(Guid tournamentId)
        {
            var bracketResults = await _appDbContext.BracketWinners.Where(x => x.Bracket.TournamentId == tournamentId && (x.FirstPlaceId != null || x.SecondPlaceId != null || x.ThirdPlaceId != null))
                .Include(x => x.Bracket.WeightCategorie.AgeGroup)
                .Include(x => x.FirstPlacePlayer.Trainer.Club)
                .Include(x => x.SecondPlacePlayer.Trainer.Club)
                .Include(x => x.ThirdPlacePlayer.Trainer.Club)
                .Include(x => x.FirstPlacePlayer.Tournament)
                .OrderBy(x => x.Bracket.WeightCategorie.AgeGroup.MinAge).ThenBy(x => x.Bracket.WeightCategorie.MaxWeight)
                .ToListAsync();
            
            return bracketResults.Select(x => new BracketWinnerViewModel()
            {
                Id = x.Id,
                Division = Join(", ",  _appDbContext.Belts.OrderBy(belt => belt.BeltNumber)
                    .Where(belt => x.Bracket.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}")),
                TournamentName = x.FirstPlacePlayer.Tournament.Name,
                FirstPlayerClubName = x.FirstPlacePlayer?.Trainer?.Club?.Name ?? Empty,
                FirstPlayerFullName = $"{x.FirstPlacePlayer?.Surname} {x.FirstPlacePlayer?.Name}",
                SecondPlayerClubName = x.SecondPlacePlayer?.Trainer?.Club?.Name ?? Empty,
                SecondPlayerFullName = $"{x.SecondPlacePlayer?.Surname} {x.SecondPlacePlayer?.Name}",
                ThirdPlayerClubName = x.ThirdPlacePlayer?.Trainer?.Club?.Name ?? Empty,
                ThirdPlayerFullName = $"{x.ThirdPlacePlayer?.Surname} {x.ThirdPlacePlayer?.Name}",
                CategorieName = $"{x.FirstPlacePlayer?.WeightCategorie?.AgeGroup?.Name} - {x.FirstPlacePlayer?.WeightCategorie?.WeightName}"
            }).ToList();
        }
    }
}
