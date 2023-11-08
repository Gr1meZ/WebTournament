using DataAccess.Domain.Models;
using Infrastructure.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IApplicationDbContext appDbContext;

        public TournamentService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddTournament(TournamentViewModel tournamentViewModel)
        {
            if (tournamentViewModel == null)
                throw new ValidationException("Tournament model is null");

            var tournament = new Tournament()
            {
                Address = tournamentViewModel.Address,
                Name = tournamentViewModel.Name,
                StartDate = DateTime.Now
            };

            appDbContext.Tournaments.Add(tournament);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteTournament(Guid id)
        {
            var tournament = await appDbContext.Tournaments.FindAsync(id);

            if (tournament == null)
                throw new ValidationException("Tournament not found");
            appDbContext.Tournaments.Remove(tournament);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditTournament(TournamentViewModel tournamentViewModel)
        {
            if (tournamentViewModel == null)
                throw new ValidationException("Tournament model is null");

            var tournament = await appDbContext.Tournaments.FindAsync(tournamentViewModel.Id);


            tournament.Name = tournamentViewModel.Name;
            tournament.Address = tournamentViewModel.Address;
            tournament.StartDate = tournamentViewModel.StartDate;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<TournamentViewModel> GetTournament(Guid id)
        {
            var tournament = await appDbContext.Tournaments.FindAsync(id);
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
            var tournaments = appDbContext.Tournaments.AsNoTracking();

            return await tournaments.Select(x => new TournamentViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                StartDate = x.StartDate
            }).ToListAsync();
        }

        public async Task<PagedResponse<TournamentViewModel[]>> TournamentsList(PagedRequest request)
        {
            var dbQuery = appDbContext.Tournaments
              .AsQueryable()
              .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.StartDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Address.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
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
    }
}
