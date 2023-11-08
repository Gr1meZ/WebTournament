using DataAccess.Common.Enums;
using DataAccess.Common.Extensions;
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
    public class FighterService : IFighterService
    {
        private readonly IApplicationDbContext appDbContext;

        public FighterService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddFighter(FighterViewModel fighterViewModel)
        {
            if (fighterViewModel == null)
                throw new ValidationException("Fighter model is null");

            var fighter = new Fighter()
            {
                Age = fighterViewModel.Age,
                BeltId = fighterViewModel.BeltId,
                City = fighterViewModel.City,
                Country = fighterViewModel.Country,
                Gender = fighterViewModel.Gender.ParseEnum<Gender>(),
                Surname = fighterViewModel.Surname,
                Name = fighterViewModel.Name,
                TournamentId = fighterViewModel.TournamentId,
                TrainerId = fighterViewModel.TrainerId,
                WeightCategorieId = fighterViewModel.WeightCategorieId
            };

            appDbContext.Fighters.Add(fighter);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteFighter(Guid id)
        {
            var fighter = await appDbContext.Fighters.FindAsync(id);

            if (fighter == null)
                throw new ValidationException("Fighter not found");
            appDbContext.Fighters.Remove(fighter);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditFighter(FighterViewModel fighterViewModel)
        {
            if (fighterViewModel == null)
                throw new ValidationException("Fighter model is null");

            var fighter = await appDbContext.Fighters.FindAsync(fighterViewModel.Id);


            fighter.Name = fighterViewModel.Name;
            fighter.Age = fighterViewModel.Age;
            fighter.BeltId = fighterViewModel.BeltId;
            fighter.City = fighterViewModel.City;
            fighter.Country = fighterViewModel.Country;
            fighter.Gender = fighterViewModel.Gender.ParseEnum<Gender>();
            fighter.Surname = fighterViewModel.Surname;
            fighter.TournamentId = fighterViewModel.TournamentId;
            fighter.TrainerId = fighterViewModel.TrainerId;
            fighter.WeightCategorieId = fighterViewModel.WeightCategorieId;

            await appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<FighterViewModel[]>> FightersList(PagedRequest request)
        {
            var dbQuery = appDbContext.Fighters
              .AsQueryable()
              .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Age.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.City.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Country.ToLower().Contains(searchWord.ToLower()) ||
                        f.Gender.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Belt.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                        f.Tournament.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Patronymic.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower())
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
                    "age" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Age)
                    : dbQuery.OrderByDescending(o => o.Age),
                    "city" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.City)
                        : dbQuery.OrderByDescending(o => o.City),
                    "surname" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Surname)
                    : dbQuery.OrderByDescending(o => o.Surname),
                    "country" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Country)
                    : dbQuery.OrderByDescending(o => o.Country),
                    "gender" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Gender)
                    : dbQuery.OrderByDescending(o => o.Gender),
                    "beltName" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Belt.ShortName)
                    : dbQuery.OrderByDescending(o => o.Belt.ShortName),
                    "tournament" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Tournament.Name)
                    : dbQuery.OrderByDescending(o => o.Tournament.Name),
                    "trainer" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Trainer.Surname)
                    : dbQuery.OrderByDescending(o => o.Trainer.Surname),
                    "weightCategorie" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.WeightCategorie.WeightName)
                    : dbQuery.OrderByDescending(o => o.WeightCategorie.WeightName),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new FighterViewModel()
            {
                Id = x.Id,
                Age = x.Age,
                BeltId = x.BeltId,
                BeltName = x.Belt.ShortName,
                City = x.City,
                Surname = x.Surname,
                Country = x.Country,
                Gender = x.Gender.MapToString(),
                Name = x.Name,
                TournamentId = x.TournamentId,
                TournamentName = x.Tournament.Name,
                TrainerId = x.TrainerId,
                TrainerName = $"{x.Trainer.Surname} {x.Trainer.Name[0]}.{x.Trainer.Patronymic[0]}"
                
            }).ToArrayAsync();

            return new PagedResponse<FighterViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task<FighterViewModel> GetFighter(Guid id)
        {
            var fighter = await appDbContext.Fighters.FindAsync(id);
            var viewModel = new FighterViewModel()
            {
                Id = fighter.Id,
                Age = fighter.Age,
                Gender = fighter.Gender.MapToString(),
                WeightCategorieId = fighter.WeightCategorieId,
                Name = fighter.Name,
                TrainerId = fighter.TrainerId,
                TournamentId= fighter.TournamentId,
                Surname= fighter.Surname,
                BeltId = fighter.BeltId,
                BeltName = fighter.Belt.ShortName,
                City = fighter.City,
                Country = fighter.Country,
                TournamentName= fighter.Tournament.Name,
                TrainerName= $"{fighter.Trainer.Surname} {fighter.Trainer.Name[0]}.{fighter.Trainer.Patronymic}",
                WeightCategorieName = fighter.WeightCategorie.WeightName

            };

            return viewModel;
        }

        public async Task<List<FighterViewModel>> GetFighters()
        {
            var fighters = appDbContext.Fighters.AsNoTracking();

            return await fighters.Select(fighter => new FighterViewModel()
            {
                Id = fighter.Id,
                Age = fighter.Age,
                Gender = fighter.Gender.MapToString(),
                WeightCategorieId = fighter.WeightCategorieId,
                Name = fighter.Name,
                TrainerId = fighter.TrainerId,
                TournamentId = fighter.TournamentId,
                Surname = fighter.Surname,
                BeltId = fighter.BeltId,
                BeltName = fighter.Belt.ShortName,
                City = fighter.City,
                Country = fighter.Country,
                TournamentName = fighter.Tournament.Name,
                TrainerName = $"{fighter.Trainer.Surname} {fighter.Trainer.Name[0]}.{fighter.Trainer.Patronymic}",
                WeightCategorieName = fighter.WeightCategorie.WeightName
            }).ToListAsync();
        }
    }
}
