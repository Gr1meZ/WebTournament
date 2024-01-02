using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup.Rules;

public class AgeGroupMustBeUniqueRule : IBusinessRule
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly int? _minAge;
    private readonly int? _maxAge;

    public AgeGroupMustBeUniqueRule(IAgeGroupRepository ageGroupRepository, int? minAge, int? maxAge)
    {
        _ageGroupRepository = ageGroupRepository;
        _minAge = minAge;
        _maxAge = maxAge;
    }

    public async Task<bool> IsBrokenAsync() => await _ageGroupRepository.IsUniqueAsync(_minAge, _maxAge);


    public string Message => "Данная возрастная группа уже существует!";
}