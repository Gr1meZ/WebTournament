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
    public class ClubService : IClubService
    {
        private readonly IApplicationDbContext appDbContext;

        public ClubService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddClub(ClubViewModel clubViewModel)
        {
            if (clubViewModel == null)
                throw new ValidationException("Club model is null");

            var club = new Club()
            {
                 Name = clubViewModel.Name
            };

            appDbContext.Clubs.Add(club);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<ClubViewModel[]>> ClubList(PagedRequest request)
        {
            var dbQuery = appDbContext.Clubs
              .AsQueryable()
              .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f => f.Name.ToLower().Contains(searchWord.ToLower())));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Name)
                        : dbQuery.OrderByDescending(o => o.Name),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new ClubViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToArrayAsync();

            return new PagedResponse<ClubViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task DeleteClub(Guid id)
        {
            var club = await appDbContext.Clubs.FindAsync(id);

            if (club == null)
                throw new ValidationException("Club not found");
            appDbContext.Clubs.Remove(club);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditClub(ClubViewModel clubViewModel)
        {
            if (clubViewModel == null)
                throw new ValidationException("Club model is null");

            var club = await appDbContext.Clubs.FindAsync(clubViewModel.Id);

            club.Name = clubViewModel.Name;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<ClubViewModel> GetClub(Guid id)
        {
            var club = await appDbContext.Clubs.FindAsync(id);
            var viewModel = new ClubViewModel()
            {
                Id = club.Id,
                Name = club.Name,
            };

            return viewModel;
        }

        public async Task<List<ClubViewModel>> GetClubs()
        {
            var clubs = appDbContext.Clubs.AsNoTracking();

            return await clubs.Select(x => new ClubViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }
    }
}
