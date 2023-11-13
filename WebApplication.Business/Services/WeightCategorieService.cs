﻿using DataAccess.Domain.Models;
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
    public class WeightCategorieService : IWeightCategorieService
    {
        private readonly IApplicationDbContext appDbContext;

        public WeightCategorieService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddWeightCategorie(WeightCategorieViewModel weightCategorieViewModel)
        {
            if (weightCategorieViewModel == null)
                throw new ValidationException("Weight categorie model is null");

            var weightCategorie = new WeightCategorie()
            {
                AgeGroupId = weightCategorieViewModel.AgeGroupId,
                MaxWeight = weightCategorieViewModel.MaxWeight,
                WeightName = weightCategorieViewModel.WeightName
            };

            appDbContext.WeightCategories.Add(weightCategorie);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteWeightCategorie(Guid id)
        {
            var weightCategorie = await appDbContext.WeightCategories.FindAsync(id) ?? throw new ValidationException("Weight categorie not found");
            appDbContext.WeightCategories.Remove(weightCategorie);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditWeightCategorie(WeightCategorieViewModel weightCategorieViewModel)
        {
            if (weightCategorieViewModel == null)
                throw new ValidationException("Weight categorie model is null");

            var weightCategorie = await appDbContext.WeightCategories.FindAsync(weightCategorieViewModel.Id);

            weightCategorie.AgeGroupId = weightCategorieViewModel.AgeGroupId;
            weightCategorie.WeightName = weightCategorieViewModel.WeightName;
            weightCategorie.MaxWeight = weightCategorieViewModel.MaxWeight;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<WeightCategorieViewModel> GetWeightCategorie(Guid id)
        {
            var weightCategorie = await appDbContext.WeightCategories.Include(x => x.AgeGroup).FirstOrDefaultAsync(x => x.Id == id);
            var viewModel = new WeightCategorieViewModel()
            {
                Id = weightCategorie.Id,
                AgeGroupId = weightCategorie.AgeGroupId,
                AgeGroupName = weightCategorie.AgeGroup.Name,
                MaxWeight = weightCategorie.MaxWeight,
                WeightName = weightCategorie.WeightName
            };

            return viewModel;
        }

        public async Task<List<WeightCategorieViewModel>> GetWeightCategories()
        {
            var weightCategories = appDbContext.WeightCategories.AsNoTracking();

            return await weightCategories.Select(weightCategorieViewModel => new WeightCategorieViewModel()
            {
                Id = weightCategorieViewModel.Id,
                AgeGroupId = weightCategorieViewModel.AgeGroupId,
                AgeGroupName = weightCategorieViewModel.WeightName,
                MaxWeight = weightCategorieViewModel.MaxWeight,
                WeightName = weightCategorieViewModel.WeightName
            }).ToListAsync();
        }

        public async Task<PagedResponse<WeightCategorieViewModel[]>> WeightCategoriesList(PagedRequest request)
        {
            var dbQuery = appDbContext.WeightCategories
               .Include(x => x.AgeGroup)
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.AgeGroup.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.MaxWeight.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightName.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "weightName" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.WeightName)
                        : dbQuery.OrderByDescending(o => o.WeightName),
                    "maxWeight" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.MaxWeight)
                    : dbQuery.OrderByDescending(o => o.MaxWeight),
                    "ageGroupName" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.AgeGroup.Name)
                        : dbQuery.OrderByDescending(o => o.AgeGroup.Name),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = await dbQuery.CountAsync();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new WeightCategorieViewModel()
            {
                Id = x.Id,
                AgeGroupId = x.AgeGroupId,
                MaxWeight = x.MaxWeight,
                WeightName = x.WeightName,
                AgeGroupName = x.AgeGroup.Name
            }).ToArrayAsync();

            return new PagedResponse<WeightCategorieViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task<Select2Response> GetAutoCompleteWeightCategories(Select2Request request)
        {
            var ageGroups = appDbContext.WeightCategories
             .AsNoTracking()
             .AsQueryable();

            var dbQuery = ageGroups;
            var total = await ageGroups.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                dbQuery = dbQuery.Where(x => x.WeightName.ToLower().Contains(request.Search.ToLower()));
            }

            if (request.PageSize != -1)
                dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

            var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.WeightName
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
