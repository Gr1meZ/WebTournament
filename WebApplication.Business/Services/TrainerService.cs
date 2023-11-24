using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using DataAccess.Abstract;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly IApplicationDbContext _appDbContext;

        public TrainerService(IApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddTrainer(TrainerViewModel trainerViewModel)
        {
            if (trainerViewModel == null)
                throw new ValidationException("Trainer model is null");

            var ageGroup = new Trainer()
            {
                Name = trainerViewModel.Name,
                ClubId = trainerViewModel.ClubId ?? Guid.Empty,
                Patronymic = trainerViewModel.Patronymic,
                Phone = trainerViewModel.Phone,
                Surname = trainerViewModel.Surname
            };

            _appDbContext.Trainers.Add(ageGroup);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteTrainer(Guid id)
        {
            var trainer = await _appDbContext.Trainers.FindAsync(id) ?? throw new ValidationException("Trainer not found");
            _appDbContext.Trainers.Remove(trainer);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditTrainer(TrainerViewModel trainerViewModel)
        {
            if (trainerViewModel == null)
                throw new ValidationException("Trainer model is null");

            var trainer = await _appDbContext.Trainers.Include(x => x.Club).FirstOrDefaultAsync(x => x.Id == trainerViewModel.Id);


            trainer!.Name = trainerViewModel.Name;
            trainer.Surname = trainerViewModel.Surname;
            trainer.Patronymic = trainerViewModel.Patronymic;
            trainer.ClubId = trainerViewModel.ClubId ?? Guid.Empty;
            trainer.Phone = trainerViewModel.Phone;
            
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<TrainerViewModel> GetTrainer(Guid id)
        {
            var trainer = await _appDbContext.Trainers.Include(x => x.Club).FirstOrDefaultAsync(y => y.Id == id);
            
            if (trainer == null)
                throw new ValidationException("Trainer not found");
            
            var viewModel = new TrainerViewModel()
            {
                Id = trainer.Id,
                ClubId = trainer.ClubId,
                Phone = trainer.Phone,
                Patronymic = trainer.Patronymic,
                Surname = trainer.Surname,
                Name = trainer.Name,
                ClubName = trainer.Club.Name
            };

            return viewModel;
        }

        public async Task<List<TrainerViewModel>> GetTrainers()
        {
            var trainer = _appDbContext.Trainers.AsNoTracking();

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
            var dbQuery = _appDbContext.Trainers
                .Include( x => x.Club)
               .AsQueryable()
               .AsNoTracking();

            // searching
            var lowerQ = request.Search?.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Patronymic.ToLower().Contains(searchWord.ToLower()) ||
                        f.Phone.ToLower().Contains(searchWord.ToLower()) ||
                        f.Club.Name.ToLower().Contains(searchWord.ToLower())
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
                Surname = x.Surname,
                ClubName = x.Club.Name
            }).ToArrayAsync();

            return new PagedResponse<TrainerViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task<Select2Response> GetAutoCompleteTrainers(Select2Request request)
        {
            var ageGroups = _appDbContext.Trainers
              .AsNoTracking()
              .AsQueryable();

            var dbQuery = ageGroups;
            var total = await ageGroups.CountAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()) ||
                x.Surname.ToLower().Contains(request.Search.ToLower()) ||
                x.Patronymic.ToLower().Contains(request.Search.ToLower()));
            }

            if (request.PageSize != -1)
                dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

            var data = dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname} {x.Name[0]}.{x.Patronymic}"
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
