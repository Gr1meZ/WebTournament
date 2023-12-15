using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DataAccess.Abstract;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class ClubService : IClubService
    {
        private readonly IApplicationDbContext _appDbContext;

        public ClubService(IApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task AddClubAsync(ClubViewModel clubViewModel)
        {
            if (clubViewModel == null)
                throw new ValidationException("Club model is null");
            
            var beltExists = await _appDbContext.Clubs
                .Where(x => x.Name == clubViewModel.Name )
                .AnyAsync();

            if (beltExists)
                throw new DataAccess.Common.Exceptions.ValidationException("ValidationException",
                    "Данный клуб уже существует!");
            
            var club = new Club()
            {
                 Name = clubViewModel.Name
            };

            _appDbContext.Clubs.Add(club);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<ClubViewModel[]>> ClubListAsync(PagedRequest request)
        {
            var dbQuery = _appDbContext.Clubs
              .AsQueryable()
              .AsNoTracking();

            var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f => f.Name.ToLower().Contains(searchWord.ToLower())));
            }

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

            var totalItemCount = dbQuery.Count();

            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new ClubViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToArrayAsync();

            return new PagedResponse<ClubViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task DeleteClubAsync(Guid id)
        {
            var club = await _appDbContext.Clubs.FindAsync(id);

            if (club == null)
                throw new ValidationException("Club not found");
            _appDbContext.Clubs.Remove(club);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditClubAsync(ClubViewModel clubViewModel)
        {
            if (clubViewModel == null)
                throw new ValidationException("Club model is null");

            var club = await _appDbContext.Clubs.FindAsync(clubViewModel.Id);

            club!.Name = clubViewModel.Name;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ClubViewModel> GetClubAsync(Guid id)
        {
            var club = await _appDbContext.Clubs.FindAsync(id);
            
            if (club == null)
                throw new ValidationException("Club not found");
            
            var viewModel = new ClubViewModel()
            {
                Id = club.Id,
                Name = club.Name,
            };

            return viewModel;
        }

        public async Task<List<ClubViewModel>> GetClubs()
        {
            var clubs = _appDbContext.Clubs.AsNoTracking();

            return await clubs.Select(x => new ClubViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        }

        public async Task<Select2Response> GetSelect2ClubsAsync(Select2Request request)
        {
            var clubs = _appDbContext.Clubs
              .AsNoTracking()
              .AsQueryable();

            var dbQuery = clubs;
            var total = await clubs.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
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
    }
}
