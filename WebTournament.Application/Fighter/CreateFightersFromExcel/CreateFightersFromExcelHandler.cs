using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.Fighter.CreateFightersFromExcel.ExcelMapper;
using WebTournament.Application.Fighter.CreateFightersFromExcel.Validators;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.CreateFightersFromExcel;

public class CreateFightersFromExcelHandler : ICommandHandler<CreateFightersFromExcelCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IClubRepository _clubRepository;
    private readonly IBeltRepository _beltRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFightersFromExcelHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork, IWeightCategorieRepository weightCategorieRepository, IAgeGroupRepository ageGroupRepository, ITrainerRepository trainerRepository, IClubRepository clubRepository, IBeltRepository beltRepository)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
        _weightCategorieRepository = weightCategorieRepository;
        _ageGroupRepository = ageGroupRepository;
        _trainerRepository = trainerRepository;
        _clubRepository = clubRepository;
        _beltRepository = beltRepository;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task Handle(CreateFightersFromExcelCommand request, CancellationToken cancellationToken)
    {
            ExcelFileValidator.ValidateFile(request.ExcelFile);
            
            using var stream = new MemoryStream();

            await request.ExcelFile.CopyToAsync(stream, cancellationToken);

            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets[0];
            
            ExcelFileValidator.ValidateWorkSheet(worksheet);
            
            var rowCount = worksheet.Dimension.Rows;
            var fightersList = new List<FighterResponse>();
            for (var row = 2; row <= rowCount; row++)
            {
                var fighterDto = new FighterResponse();
                
                ExcelFightersMapper.ParseExcelFile(worksheet, row, fighterDto);
                if (fighterDto?.Gender == null)
                    continue;
                
                fighterDto.TournamentId = request.Id;
                fighterDto.Age = AgeCalculator.CalculateAge(fighterDto.BirthDate);
                fighterDto.WeightCategorieId = await GetFightersWeightCategorieIdAsync(fighterDto.WeightNumber,
                    fighterDto.Gender, fighterDto.BirthDate);
                
                fighterDto.TrainerId = await GetFightersTrainerIdAsync(fighterDto.TrainerName!, fighterDto.ClubName!);
                
                fighterDto.BeltId = await FindFightersBeltIdAsync(fighterDto.BeltNumber, fighterDto.BeltShortName!);
                
                fightersList.Add(fighterDto);
                
            }

            var domainFighters = new List<Domain.Objects.Fighter.Fighter>();
            
            foreach (var fighterDto in fightersList)
            {
                var domainFighter = await Domain.Objects.Fighter.Fighter.CreateAsync(Guid.NewGuid(), fighterDto.TournamentId.Value, fighterDto.WeightCategorieId.Value,
                    fighterDto.BeltId.Value, fighterDto.TrainerId.Value, null, fighterDto.Name, fighterDto.Surname, fighterDto.BirthDate, fighterDto.Country,
                    fighterDto.City, fighterDto.Gender, _fighterRepository);
                domainFighters.Add(domainFighter);
            }

            await _fighterRepository.AddRangeAsync(domainFighters);
            await _unitOfWork.CommitAsync(cancellationToken);
    }
    
    private async Task<Guid> FindFightersBeltIdAsync(int beltNumber, string beltShortName)
    {
        var belt = await _beltRepository.GetAll()
            .FirstOrDefaultAsync(x => x.BeltNumber == beltNumber && x.ShortName == beltShortName);

        if (belt is null)
            throw new ValidationException("ValidationException", $"Пояс {beltNumber} {beltShortName} не найден. Добавьте данный пояс в базу данных!");
            
        return belt.Id;
    }
    
    private async Task<Guid> GetFightersWeightCategorieIdAsync(int weight, string gender, DateTime birthDate)
    {
        var age = AgeCalculator.CalculateAge(birthDate);
        var ageGroup = await _ageGroupRepository.GetAll().FirstOrDefaultAsync(x => age >= x.MinAge && age <= x.MaxAge);
           
        if (ageGroup is null)
            throw new ValidationException("ValidationException",$"Возрастная группа для возраста '{age}' не найдена. Создайте в базе данных категорию для данного возраста!");
            
        var weightCategorie = await _weightCategorieRepository.GetAll()
            .Where(x => weight <= x.MaxWeight && x.AgeGroupId == ageGroup.Id && x.Gender == GenderExtension.ParseEnum(gender)) 
            .OrderBy(x => x.MaxWeight) 
            .FirstOrDefaultAsync();
          
        if (weightCategorie is null)
            throw new ValidationException("ValidationException",$"Весовая категория для возрастной группы {ageGroup.Name} с весом спортсмена {weight} кг не найдена! Добавьте весовую категорию в базу данных!");
        return weightCategorie.Id;
    }
    private async Task<Guid> GetFightersTrainerIdAsync(string trainerFullName, string clubName)
    {
        var fullNameArray = trainerFullName.Split(" ");
        var trainersSurname = fullNameArray[0];
        var trainersName = fullNameArray[1];
        var trainersPatronymic = fullNameArray[2];

        var trainer = await _trainerRepository.GetAll().FirstOrDefaultAsync(x => x.Surname == trainersSurname && x.Name == trainersName && x.Patronymic == trainersPatronymic);
        var club = await _clubRepository.GetAll().FirstOrDefaultAsync(x => x.Name == clubName);
            
        if (trainer is null)
            throw new ValidationException("ValidationException",$"Тренер {trainerFullName} не найден");
            
        if (club is null)
            throw new ValidationException("ValidationException",$"Клуб {clubName} не найден");
            
        if (trainer.ClubId != club.Id)
            throw new ValidationException("ValidationException",$"Ошибка связи между тренером {trainer.Surname} и клубом {club.Name}. Проверьте, привязан ли клуб к данному тренеру!");
            
        return trainer.Id;
    }
}