using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Exceptions;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.Application.Fighter.CreateFightersFromExcel.FightersObjectsGetters;

internal class WeightCategorieGetter
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IAgeGroupRepository _ageGroupRepository;


    public WeightCategorieGetter(IWeightCategorieRepository weightCategorieRepository, IAgeGroupRepository ageGroupRepository)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<Guid> GetWeightCategorieIdAsync(int weight, string gender, DateTime birthDate)
    {
        var age = AgeCalculator.CalculateAge(birthDate);
        var ageGroup = await _ageGroupRepository.GetAll().FirstOrDefaultAsync(x => age >= x.MinAge && age <= x.MaxAge);
           
        if (ageGroup is null)
            throw new ValidationException("ValidationException",$"Возрастная группа для возраста '{age}' не найдена. Создайте в базе данных категорию для данного возраста!");
            
        var weightCategorie = await _weightCategorieRepository.GetAll()
            .Where(x => weight <= x.MaxWeight && x.AgeGroupId == ageGroup.Id && x.Gender == GenderExtension.ParseEnum(gender)) // Фильтруем по условию
            .OrderBy(x => x.MaxWeight) 
            .FirstOrDefaultAsync();
          
        if (weightCategorie is null)
            throw new ValidationException("ValidationException",$"Весовая категория для возрастной группы {ageGroup.Name} с весом спортсмена {weight} кг не найдена! Добавьте весовую категорию в базу данных!");
        
        return weightCategorie.Id;
    }
}