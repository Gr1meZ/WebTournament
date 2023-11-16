using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using DataAccess.Abstract;
using DataAccess.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using WebTournament.Business.Abstract;
using WebTournament.Business.Helpers;
using WebTournament.Models;

namespace WebTournament.Business.Services
{
    public class ExcelParserService : IExcelParserService
    {
        private readonly IApplicationDbContext _appDbContext;
        private readonly IFighterService _fighterService;

        public ExcelParserService(IApplicationDbContext appDbContext, IFighterService fighterService) {
            
            _appDbContext = appDbContext;
            _fighterService = fighterService;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task GenerateFromExcelAsync(IFormFile excelFile, Guid tournamentId, CancellationToken cancellationToken)
        {

            using var stream = new MemoryStream();

            await excelFile.CopyToAsync(stream, cancellationToken);

            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets[0]; 
            var rowCount = worksheet.Dimension.Rows;

            for (var row = 2; row <= rowCount; row++)
            {
                var fighterViewModel = new FighterViewModel();
                for (var col = 1; col <= 11; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Value;
                    var columnHeader = worksheet.Cells[1, col].Value.ToString();
                    if (columnHeader == null || cellValue == null)
                        continue;
                    
                    switch (columnHeader)
                    {
                        case "Фамилия":
                            fighterViewModel.Surname = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Имя":
                            fighterViewModel.Name = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Дата рождения":
                            fighterViewModel.BirthDate = Convert.ToDateTime(cellValue);
                            break;
                        case "Номер пояса":
                            fighterViewModel.BeltNumber = Convert.ToInt32(cellValue);
                            break;
                        case "Ступень":
                            fighterViewModel.BeltShortName = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Страна":
                            fighterViewModel.Country = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Город":
                            fighterViewModel.City = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Пол":
                            fighterViewModel.Gender = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Вес":
                            fighterViewModel.WeightNumber = Convert.ToInt32(cellValue);
                            break;
                        case "Тренер":
                            fighterViewModel.TrainerName = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Клуб":
                            fighterViewModel.ClubName = cellValue.ToString() ?? string.Empty;
                            break;
                    }
                    
                }
                await CheckIfFighterExists(fighterViewModel);

                fighterViewModel.Age = AgeCalculator.CalculateAge(fighterViewModel.BirthDate);
                fighterViewModel.WeightCategorieId = await GetWeightCategorieId(fighterViewModel.WeightNumber,
                    fighterViewModel.Gender, fighterViewModel.BirthDate);
                fighterViewModel.TrainerId =
                    await GetTrainerId(fighterViewModel.TrainerName, fighterViewModel.ClubName);
                fighterViewModel.BeltId = await FindBeltId(fighterViewModel.BeltNumber, fighterViewModel.BeltShortName);
                fighterViewModel.TournamentId = tournamentId;
                
                await _fighterService.AddFighter(fighterViewModel);
            }
        }

        private async Task CheckIfFighterExists(FighterViewModel fighterViewModel)
        {
            var isFighterExists = await _appDbContext.Fighters.AnyAsync(x =>
                x.Surname == fighterViewModel.Surname && x.Name == fighterViewModel.Name && x.BirthDate == fighterViewModel.BirthDate);
            if (isFighterExists)
                throw new ValidationException($"Игрок {fighterViewModel.Surname} {fighterViewModel.Name} уже существует");
        }
        
        private async Task<Guid> GetWeightCategorieId(int weight, string gender, DateTime birthDate)
        {
            var age = AgeCalculator.CalculateAge(birthDate);
            var ageGroup = await _appDbContext.AgeGroups.FirstOrDefaultAsync(x => age >= x.MinAge && age <= x.MaxAge);
           
            if (ageGroup == null)
                throw new ValidationException("Возрастная категория не найдена");
            
            var weightCategorie = await _appDbContext.WeightCategories.FirstOrDefaultAsync(x => x.MaxWeight > weight && x.AgeGroupId == ageGroup.Id && x.Gender == GenderExtension.ParseEnum(gender));
          
            if (weightCategorie == null)
                throw new ValidationException("Весовая категория не найдена");
            return weightCategorie.Id;
        }
        private async Task<Guid> FindBeltId(int beltNumber, string beltShortName)
        {
            var belt = await _appDbContext.Belts
                .FirstOrDefaultAsync(x => x.BeltNumber == beltNumber && x.ShortName == beltShortName);
            
            if (belt == null)
                throw new ValidationException("Пояс не найден");
            
            return belt.Id;
        }

        private async Task<Guid> GetTrainerId(string trainerFullName, string clubName)
        {
            var fullNameArray = trainerFullName.Split(" ");
            var trainersSurname = fullNameArray[0];
            var trainersName = fullNameArray[1];
            var trainersPatronymic = fullNameArray[2];

            var trainer = await _appDbContext.Trainers.FirstOrDefaultAsync(x => x.Surname == trainersSurname && x.Name == trainersName && x.Patronymic == trainersPatronymic);
            var club = await _appDbContext.Clubs.FirstOrDefaultAsync(x => x.Name == clubName);
            
            if (trainer == null)
                throw new ValidationException("Тренер не найден");
            
            if (club == null)
                throw new ValidationException("Клуб не найден");
            
            if (trainer.ClubId != club.Id)
                throw new ValidationException($"Ошибка связи между {trainer.Surname} и {club.Name}");
            
            return trainer.Id;
        }

     
    }

}

