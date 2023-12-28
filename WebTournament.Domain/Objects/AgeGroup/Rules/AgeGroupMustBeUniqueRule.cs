using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup.Rules;

public class AgeGroupMustBeUniqueRule : IBusinessRule
{

    private readonly bool _isUnique;
    public AgeGroupMustBeUniqueRule(bool isUnique)
    {
        this._isUnique = isUnique;
    }

    public bool IsBroken() => _isUnique;


    public string Message => "Данная возрастная группа уже существует!";
}