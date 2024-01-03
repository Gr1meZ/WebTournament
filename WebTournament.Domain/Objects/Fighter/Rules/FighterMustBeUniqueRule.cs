using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Fighter.Rules;

public class FighterMustBeUniqueRule : IBusinessRule
{
    private readonly IFighterRepository _fighterRepository;
    private readonly string _surname;
    private readonly string _name;
    private readonly string _city;
    private readonly Guid _tournamentId;

    public FighterMustBeUniqueRule(IFighterRepository fighterRepository, string surname, string name, string city, Guid tournamentId)
    {
        _fighterRepository = fighterRepository;
        _surname = surname;
        _name = name;
        _city = city;
        _tournamentId = tournamentId;
    }

    public async Task<bool> IsBrokenAsync() => await _fighterRepository.IsUnique(_surname, _name, _city, _tournamentId);


    public string Message => $"Спортсмен {_surname} {_name} уже существует!";
}