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
    
    public class BeltService : IBeltService
    {
        private readonly IApplicationDbContext appDbContext;

        public BeltService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddBelt(BeltViewModel beltViewModel)
        {
            if (beltViewModel == null)
                throw new ValidationException("Belt model is null");

            var belt = new Belt()
            {
                BeltNumber = beltViewModel.BeltNumber,
                FullName = beltViewModel.FullName,
                ShortName = beltViewModel.ShortName,
            };

            appDbContext.Belts.Add(belt);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<BeltViewModel[]>> BeltList(PagedRequest request)
        {
            var dbQuery = appDbContext.Belts
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                        f.FullName.ToLower().Contains(searchWord.ToLower()) ||
                        f.BeltNumber.ToString().ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "shortName" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.ShortName)
                        : dbQuery.OrderByDescending(o => o.ShortName),
                    "fullName" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.FullName)
                    : dbQuery.OrderByDescending(o => o.FullName),
                    "beltNumber" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.BeltNumber)
                        : dbQuery.OrderByDescending(o => o.BeltNumber),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new BeltViewModel()
            {
                Id = x.Id,
                BeltNumber = x.BeltNumber,
                FullName = x.FullName,
                ShortName = x.ShortName
            }).ToArrayAsync();

            return new PagedResponse<BeltViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task DeleteBelt(Guid id)
        {
            var belt = await appDbContext.Belts.FindAsync(id) ?? throw new ValidationException("Belt not found");
            appDbContext.Belts.Remove(belt);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditBelt(BeltViewModel beltViewModel)
        {
            if (beltViewModel == null)
                throw new ValidationException("Age group model is null");

            var belt = await appDbContext.Belts.FindAsync(beltViewModel.Id);


            belt.ShortName = beltViewModel.ShortName;
            belt.BeltNumber = beltViewModel.BeltNumber;
            belt.FullName = beltViewModel.FullName;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<BeltViewModel> GetBelt(Guid id)
        {
            var belt = await appDbContext.Belts.FindAsync(id);
            var viewModel = new BeltViewModel()
            {
                Id = belt.Id,
                BeltNumber = belt.BeltNumber,
                FullName = belt.FullName,
                ShortName = belt.ShortName
            };

            return viewModel;
        }

        public async Task<List<BeltViewModel>> GetBelts()
        {
            var belts = appDbContext.Belts.AsNoTracking();

            return await belts.Select(x => new BeltViewModel()
            {
                Id = x.Id,
                BeltNumber = x.BeltNumber,
                FullName = x.FullName,
                ShortName = x.ShortName
            }).ToListAsync();
        }

        public async Task<Select2Response> GetAutoCompleteBelts(Select2Request request)
        {
            var belts = appDbContext.Belts
               .AsNoTracking()
               .AsQueryable();

            var dbQuery = belts;
            var total = await belts.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                dbQuery = dbQuery.Where(x => x.ShortName.ToLower().Contains(request.Search.ToLower()) || x.BeltNumber.ToString().Contains(request.Search.ToLower()));
            }

            if (request.PageSize != -1)
                dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

            var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.BeltNumber} {x.ShortName}"
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
