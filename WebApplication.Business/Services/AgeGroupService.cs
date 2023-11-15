using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DataAccess.Abstract;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class AgeGroupService : IAgeGroupService
    {
        private readonly IApplicationDbContext _appDbContext;

        public AgeGroupService(IApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext; 
        }

        public async Task AddAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {
            if (ageGroupViewModel == null)
                throw new ValidationException("Age group model is null");

            var ageGroup = new AgeGroup()
            {
                MaxAge = ageGroupViewModel.MaxAge ?? 0,
                MinAge = ageGroupViewModel.MinAge ?? 0,
                Name = ageGroupViewModel.Name
            };

            _appDbContext.AgeGroups.Add(ageGroup);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<AgeGroupViewModel[]>> AgeGroupList(PagedRequest request)
        {
            var dbQuery = _appDbContext.AgeGroups
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.MinAge.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.MaxAge.ToString().ToLower().Contains(searchWord.ToLower())
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
                    "minAge" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.MinAge)
                    : dbQuery.OrderByDescending(o => o.MinAge),
                    "maxAge" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.MaxAge)
                        : dbQuery.OrderByDescending(o => o.MaxAge),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new AgeGroupViewModel()
            {
                Id = x.Id,
                MaxAge = x.MaxAge,
                MinAge = x.MinAge,
                Name = x.Name
            }).ToArrayAsync();

            return new PagedResponse<AgeGroupViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task DeleteAgeGroup(Guid id)
        {
            var ageGroup = await _appDbContext.AgeGroups.FindAsync(id) ?? throw new ValidationException("Age group not found");
            _appDbContext.AgeGroups.Remove(ageGroup);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {

            if (ageGroupViewModel == null)
                throw new ValidationException("Age group model is null");

            var ageGroup = await _appDbContext.AgeGroups.FindAsync(ageGroupViewModel.Id);


            ageGroup!.Name = ageGroupViewModel.Name;
            ageGroup.MaxAge = ageGroupViewModel.MaxAge ?? 0;
            ageGroup.MinAge = ageGroupViewModel.MinAge ?? 0;

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<AgeGroupViewModel> GetAgeGroup(Guid id)
        {
            var ageGroup = await _appDbContext.AgeGroups.FindAsync(id);
            
            if (ageGroup == null)
                throw new ValidationException("Age group is not found");
            
            var viewModel = new AgeGroupViewModel()
            {
                Id = ageGroup.Id,
                MaxAge = ageGroup.MaxAge,
                Name = ageGroup.Name,
                MinAge = ageGroup.MinAge,

            };

            return viewModel;
        }

        public async Task<List<AgeGroupViewModel>> GetAgeGroups()
        {
            var ageGroups = _appDbContext.AgeGroups.AsNoTracking();

            return await ageGroups.Select(x => new AgeGroupViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                MaxAge = x.MaxAge,
                MinAge = x.MinAge,
            }).ToListAsync();
        }

        public async Task<Select2Response> GetAutoCompleteAgeGroups(Select2Request request)
        {
            var ageGroups = _appDbContext.AgeGroups
              .AsNoTracking()
              .AsQueryable();

            var dbQuery = ageGroups;
            var total = await ageGroups.CountAsync();

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
