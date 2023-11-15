using DataAccess.Common.Extensions;
using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;
using WebTournament.Business.Helpers;
using DataAccess.Abstract;

namespace WebTournament.Business.Services
{
    public class FighterService : IFighterService
    {
        private readonly IApplicationDbContext _appDbContext;

        public FighterService(IApplicationDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task AddFighter(FighterViewModel fighterViewModel)
        {
            if (fighterViewModel == null)
                throw new ValidationException("Fighter model is null");

            var fighter = new Fighter()
            {
                Age = AgeCalculator.CalculateAge(fighterViewModel.BirthDate),
                BirthDate = fighterViewModel.BirthDate,
                BeltId = fighterViewModel.BeltId ?? Guid.Empty,
                City = fighterViewModel.City,
                Country = fighterViewModel.Country,
                Gender = GenderExtension.ParseEnum(fighterViewModel.Gender),
                Surname = fighterViewModel.Surname,
                Name = fighterViewModel.Name,
                TournamentId = fighterViewModel.TournamentId ?? Guid.Empty,
                TrainerId = fighterViewModel.TrainerId ?? Guid.Empty,
                WeightCategorieId = fighterViewModel.WeightCategorieId ?? Guid.Empty
            };

            _appDbContext.Fighters.Add(fighter);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteFighter(Guid id)
        {
            var fighter = await _appDbContext.Fighters.FindAsync(id) ?? throw new ValidationException("Fighter not found");
            _appDbContext.Fighters.Remove(fighter);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task EditFighter(FighterViewModel fighterViewModel)
        {
            if (fighterViewModel == null)
                throw new ValidationException("Fighter model is null");

            var fighter = await _appDbContext.Fighters.FindAsync(fighterViewModel.Id);


            fighter!.Name = fighterViewModel.Name;
            fighter.BirthDate = fighterViewModel.BirthDate;
            fighter.Age = AgeCalculator.CalculateAge(fighterViewModel.BirthDate);
            fighter.BeltId = fighterViewModel.BeltId ?? Guid.Empty;
            fighter.City = fighterViewModel.City;
            fighter.Country = fighterViewModel.Country;
            fighter.Gender = GenderExtension.ParseEnum(fighterViewModel.Gender);
            fighter.Surname = fighterViewModel.Surname;
            fighter.TournamentId = fighterViewModel.TournamentId ?? Guid.Empty;
            fighter.TrainerId = fighterViewModel.TrainerId ?? Guid.Empty;
            fighter.WeightCategorieId = fighterViewModel.WeightCategorieId ?? Guid.Empty;
            
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<PagedResponse<FighterViewModel[]>> FightersList(PagedRequest request, Guid tournamentId)
        {
            var dbQuery = _appDbContext.Tournaments
              .SelectMany(x => x.Fighters)
              .Include(x => x.Trainer.Club).Include(x => x.Belt).Include(x => x.Tournament).Include(x => x.WeightCategorie.AgeGroup)
              .Where(x => x.TournamentId ==  tournamentId)
              .AsQueryable()
              .AsNoTracking();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Age.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.BirthDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.City.ToLower().Contains(searchWord.ToLower()) ||
                        f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.Country.ToLower().Contains(searchWord.ToLower()) ||
                        f.Belt.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Surname.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                        f.Trainer.Club.Name.ToLower().Contains(searchWord.ToLower())
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
                    "birthDate" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.BirthDate)
                    : dbQuery.OrderByDescending(o => o.BirthDate),
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
                    "trainerName" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.Trainer.Surname)
                    : dbQuery.OrderByDescending(o => o.Trainer.Surname),
                    "weightCategorieName" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.WeightCategorie.WeightName)
                    : dbQuery.OrderByDescending(o => o.WeightCategorie.WeightName),
                    "clubName" => (request.OrderDir.Equals("asc"))
                   ? dbQuery.OrderBy(o => o.Trainer.Club.Name)
                   : dbQuery.OrderByDescending(o => o.Trainer.Club.Name),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = await dbQuery.CountAsync();

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery.Select(x => new FighterViewModel()
            {
                Id = x.Id,
                Age = x.Age,
                BirthDate = x.BirthDate,
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
                TrainerName = $"{x.Trainer.Surname} {x.Trainer.Name[0]}.{x.Trainer.Patronymic[0]}",
                WeightCategorieName = $"{x.WeightCategorie.AgeGroup.Name} {x.WeightCategorie.WeightName}",
                ClubName = x.Trainer.Club.Name
            }).ToArrayAsync();

            return new PagedResponse<FighterViewModel[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
        }

        public async Task<FighterViewModel> GetFighter(Guid id)
        {
            var fighter = await _appDbContext.Fighters
                .Include(x => x.Tournament)
                .Include(x => x.Belt)
                .Include(x => x.Trainer)
                .Include(x => x.WeightCategorie.AgeGroup)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (fighter == null)
                throw new ValidationException("Fighter not found");
            
            var viewModel = new FighterViewModel()
            {
                Id = fighter.Id,
                Age = fighter.Age,
                BirthDate = fighter.BirthDate,
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
            var fighters = _appDbContext.Fighters.AsNoTracking();

            return await fighters.Select(fighter => new FighterViewModel()
            {
                Id = fighter.Id,
                Age = fighter.Age,
                BirthDate = fighter.BirthDate,
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
