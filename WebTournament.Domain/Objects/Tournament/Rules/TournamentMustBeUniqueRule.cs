using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Tournament.Rules;

public class TournamentMustBeUniqueRule : IBusinessRule
{
    private readonly string _address;
    private readonly string _name;
    private readonly ITournamentRepository _tournamentRepository;
    
    public TournamentMustBeUniqueRule(string address, string name, ITournamentRepository tournamentRepository)
    {
        _address = address;
        _name = name;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<bool> IsBrokenAsync() => await _tournamentRepository.IsExistsAsync(_name, _address);


    public string Message => "Данный турнир уже создан!";
}