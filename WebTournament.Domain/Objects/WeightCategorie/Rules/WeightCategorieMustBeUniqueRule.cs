using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.WeightCategorie.Rules;

public class WeightCategorieMustBeUniqueRule : IBusinessRule
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly int _maxWeight;
    private readonly string _gender;
    private readonly Guid _ageGroupId;
    public WeightCategorieMustBeUniqueRule(IWeightCategorieRepository weightCategorieRepository, int maxWeight, string gender, Guid ageGroupId)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _maxWeight = maxWeight;
        _gender = gender;
        _ageGroupId = ageGroupId;
    }

    public async Task<bool> IsBrokenAsync() => await _weightCategorieRepository.IsUnique(_maxWeight, _gender, _ageGroupId);
   

    public string Message => "Данная весовая категория уже существует!";
}