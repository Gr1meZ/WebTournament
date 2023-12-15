using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using DataAccess.Abstract;
using DataAccess.Common.Exceptions;
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
            if (excelFile is not { Length: > 0 })
                throw new ValidationException("ValidationException", "Файл пустой");

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("ValidationException", "Не поддерживаемый формат файла");
            
            using var stream = new MemoryStream();

            await excelFile.CopyToAsync(stream, cancellationToken);

            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets[0];
            if (worksheet.Name != "Заявка")
                throw new ValidationException("ValidationException", "Данный excel файл не содержит заявку на участников. Пожалуйста, добавьте правильную заявку!");
            
            RemoveEmptyRows(worksheet);
            
            var rowCount = worksheet.Dimension.Rows;
            var fightersList = new List<FighterViewModel>();
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
                            if (DateTime.TryParse(cellValue.ToString(), out var birthDate))
                                fighterViewModel.BirthDate = birthDate;
                            else
                                throw new ValidationException("ValidationException", $"Неизвестный формат даты {cellValue}");
                            break;
                        case "Номер пояса":
                            if (int.TryParse(cellValue.ToString(), out var beltNumber))
                                fighterViewModel.BeltNumber = Convert.ToInt32(beltNumber);
                            else
                                throw new ValidationException("ValidationException", $"Неизвестный формат номера пояса {cellValue}");
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
                            if (int.TryParse(cellValue.ToString(), out var weightNumber))
                                fighterViewModel.WeightNumber = Convert.ToInt32(cellValue);
                            else
                                throw new ValidationException("ValidationException", $"Неизвестный формат веса {cellValue}");
                            break;
                        case "Тренер":
                            fighterViewModel.TrainerName = cellValue.ToString() ?? string.Empty;
                            break;
                        case "Клуб":
                            fighterViewModel.ClubName = cellValue.ToString() ?? string.Empty;
                            break;
                    }
                    
                }
                fighterViewModel.TournamentId = tournamentId;
                await CheckIfFighterExistsAsync(fighterViewModel);
                
                fighterViewModel.Age = AgeCalculator.CalculateAge(fighterViewModel.BirthDate);
                fighterViewModel.WeightCategorieId = await GetWeightCategorieIdAsync(fighterViewModel.WeightNumber,
                    fighterViewModel.Gender, fighterViewModel.BirthDate);
                
                fighterViewModel.TrainerId = await GetTrainerIdAsync(fighterViewModel.TrainerName!, fighterViewModel.ClubName!);
                
                fighterViewModel.BeltId = await FindBeltIdAsync(fighterViewModel.BeltNumber, fighterViewModel.BeltShortName!);
                
                fightersList.Add(fighterViewModel);
                
            }

            foreach (var fighter in fightersList)
                await _fighterService.AddFighterAsync(fighter);
        }
        
        public void RemoveEmptyRows(ExcelWorksheet worksheet)
        {
            for (var row = worksheet.Dimension.End.Row; row >= 1; row--)
            {
                var allEmpty = true;
                for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    if (worksheet.Cells[row, col].Value == null ||
                        string.IsNullOrEmpty(worksheet.Cells[row, col].Value.ToString())) continue;
                    allEmpty = false;
                    break;
                }

                if (allEmpty)
                {
                    worksheet.DeleteRow(row);
                }
            }
        }
        
        private async Task CheckIfFighterExistsAsync(FighterViewModel fighterViewModel)
        {
            var fighter = await _appDbContext.Fighters.FirstOrDefaultAsync(x =>
                x.Surname == fighterViewModel.Surname && x.Name == fighterViewModel.Name && x.BirthDate == fighterViewModel.BirthDate && x.TournamentId == fighterViewModel.TournamentId);
            
            if (fighter != null)
                throw new ValidationException("ValidationException",$"Спортсмен {fighter.Surname} {fighter.Name} уже существует");
        }
        
        private async Task<Guid> GetWeightCategorieIdAsync(int weight, string gender, DateTime birthDate)
        {
            var age = AgeCalculator.CalculateAge(birthDate);
            var ageGroup = await _appDbContext.AgeGroups.FirstOrDefaultAsync(x => age >= x.MinAge && age <= x.MaxAge);
           
            if (ageGroup == null)
                throw new ValidationException("ValidationException",$"Возрастная группа для возраста '{age}' не найдена. Создайте в базе данных категорию для данного возраста!");
            
            var weightCategorie = await _appDbContext.WeightCategories
                .Where(x => weight <= x.MaxWeight && x.AgeGroupId == ageGroup.Id && x.Gender == GenderExtension.ParseEnum(gender)) // Фильтруем по условию
                .OrderBy(x => x.MaxWeight) 
                .FirstOrDefaultAsync();
          
            if (weightCategorie == null)
                throw new ValidationException("ValidationException",$"Весовая категория для возрастной группы {ageGroup.Name} с весом спортсмена {weight} кг не найдена! Добавьте весовую категорию в базу данных!");
            return weightCategorie.Id;
        }
        
        private async Task<Guid> FindBeltIdAsync(int beltNumber, string beltShortName)
        {
            var belt = await _appDbContext.Belts
                .FirstOrDefaultAsync(x => x.BeltNumber == beltNumber && x.ShortName == beltShortName);

            if (belt == null)
                throw new ValidationException("ValidationException", $"Пояс {beltNumber} {beltShortName} не найден. Добавьте данный пояс в базу данных!");
            
            return belt.Id;
        }

        private async Task<Guid> GetTrainerIdAsync(string trainerFullName, string clubName)
        {
            var fullNameArray = trainerFullName.Split(" ");
            var trainersSurname = fullNameArray[0];
            var trainersName = fullNameArray[1];
            var trainersPatronymic = fullNameArray[2];

            var trainer = await _appDbContext.Trainers.FirstOrDefaultAsync(x => x.Surname == trainersSurname && x.Name == trainersName && x.Patronymic == trainersPatronymic);
            var club = await _appDbContext.Clubs.FirstOrDefaultAsync(x => x.Name == clubName);
            
            if (trainer == null)
                throw new ValidationException("ValidationException",$"Тренер {trainerFullName} не найден");
            
            if (club == null)
                throw new ValidationException("ValidationException",$"Клуб {clubName} не найден");
            
            if (trainer.ClubId != club.Id)
                throw new ValidationException("ValidationException",$"Ошибка связи между тренером {trainer.Surname} и клубом {club.Name}. Проверьте, привязан ли клуб к данному тренеру!");
            
            return trainer.Id;
        }

     
    }

}

