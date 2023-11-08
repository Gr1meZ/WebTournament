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
    public class TrainerService : ITrainerService
    {
        private readonly IApplicationDbContext appDbContext;

        public TrainerService(IApplicationDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddTrainer(TrainerViewModel trainerViewModel)
        {
            if (trainerViewModel == null)
                throw new ValidationException("Trainer model is null");

            var ageGroup = new Trainer()
            {
                Name = trainerViewModel.Name,
                ClubId = trainerViewModel.ClubId,
                Patronymic = trainerViewModel.Patronymic,
                Phone = trainerViewModel.Phone,
                Surname = trainerViewModel.Surname
            };

            appDbContext.Trainers.Add(ageGroup);
            await appDbContext.SaveChangesAsync();
        }

        public async Task DeleteTrainer(Guid id)
        {
            var trainer = await appDbContext.Trainers.FindAsync(id);

            if (trainer == null)
                throw new ValidationException("Trainer not found");
            appDbContext.Trainers.Remove(trainer);

            await appDbContext.SaveChangesAsync();
        }

        public async Task EditTrainer(TrainerViewModel trainerViewModel)
        {
            if (trainerViewModel == null)
                throw new ValidationException("Trainer model is null");

            var trainer = await appDbContext.Trainers.FindAsync(trainerViewModel.Id);


            trainer.Name = trainerViewModel.Name;
            trainer.Surname = trainerViewModel.Surname;
            trainer.Patronymic = trainerViewModel.Patronymic;
            trainer.ClubId = trainerViewModel.ClubId;
            trainer.Phone = trainerViewModel.Phone;
            
            await appDbContext.SaveChangesAsync();
        }

        public async Task<TrainerViewModel> GetTrainer(Guid id)
        {
            var trainer = await appDbContext.Trainers.FindAsync(id);
            var viewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                ClubId = trainer.ClubId,
                Phone = trainer.Phone,
                Patronymic = trainer.Patronymic,
                Surname = trainer.Surname,
                Name = trainer.Name
            };

            return viewModel;
        }

        public async Task<List<TrainerViewModel>> GetTrainers()
        {
            var trainer = appDbContext.Trainers.AsNoTracking();

            return await trainer.Select(trainer => new TrainerViewModel()
            {
                Id = trainer.Id,
                ClubId = trainer.ClubId,
                Phone = trainer.Phone,
                Patronymic = trainer.Patronymic,
                Surname = trainer.Surname,
                Name = trainer.Name
            }).ToListAsync();
        }

        public async Task<PagedResponse<TrainerViewModel[]>> TrainersList(PagedRequest request)
        {
            var dbQuery = appDbContext.Trainers
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ?.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Patronymic.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Phone.ToString().ToLower().Contains(searchWord.ToLower())
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
                    "surname" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Surname)
                    : dbQuery.OrderByDescending(o => o.Surname),
                    "patronymic" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Patronymic)
                        : dbQuery.OrderByDescending(o => o.Patronymic),
                    "phone" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Phone)
                    : dbQuery.OrderByDescending(o => o.Phone),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = dbQuery.Count();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new TrainerViewModel()
            {
                Id = x.Id,
                ClubId = x.ClubId,
                Phone = x.Phone,
                Name = x.Name,
                Patronymic = x.Patronymic,
                Surname = x.Surname
            }).ToArrayAsync();

            return new PagedResponse<TrainerViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }
    }
}