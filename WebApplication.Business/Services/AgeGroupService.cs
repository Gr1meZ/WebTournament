using DataAccess.Domain.Models;
using Infrastructure.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class AgeGroupService : IAgeGroupService
    {
        private readonly IApplicationDbContext appDbContext;

        public AgeGroupService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext; 
        }

        public async Task AddAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {
            if (ageGroupViewModel == null)
                throw new ValidationException("Age group model is null");

            var ageGroup = new AgeGroup()
            {
                MaxAge = ageGroupViewModel.MaxAge,
                MinAge = ageGroupViewModel.MinAge,
                Name = ageGroupViewModel.Name
            };

            appDbContext.AgeGroups.Add(ageGroup);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<AgeGroupViewModel[]>> AgeGroupList(PagedRequest request)
        {
            var dbQuery = appDbContext.AgeGroups
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
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
            }).ToArrayAsync();

            return new PagedResponse<AgeGroupViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task DeleteAgeGroup(Guid id)
        {
            var ageGroup = await appDbContext.AgeGroups.FindAsync(id);

            if (ageGroup == null)
                throw new ValidationException("Age group not found");
            appDbContext.AgeGroups.Remove(ageGroup);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {

            if (ageGroupViewModel == null)
                throw new ValidationException("Age group model is null");

            var ageGroup = await appDbContext.AgeGroups.FindAsync(ageGroupViewModel.Id);


            ageGroup.Name = ageGroupViewModel.Name;
            ageGroup.MaxAge = ageGroupViewModel.MaxAge;
            ageGroup.MinAge = ageGroupViewModel.MinAge;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<AgeGroupViewModel> GetAgeGroup(Guid id)
        {
            var ageGroup = await appDbContext.AgeGroups.FindAsync(id);
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
            var ageGroups = appDbContext.AgeGroups.AsNoTracking();

            return await ageGroups.Select(x => new AgeGroupViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                MaxAge = x.MaxAge,
                MinAge = x.MinAge,
            }).ToListAsync();
        }
    }
}
