using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Bracket.Rules;

public class BracketMustBeUniqueRule : IBusinessRule
{
    private readonly Guid _tournamentId;
    private readonly Guid _weightCategorieId;
    private readonly Guid[] _division;
    private readonly IBracketRepository _bracketRepository;

    public BracketMustBeUniqueRule(Guid tournamentId, Guid weightCategorieId, Guid[] division,
        IBracketRepository bracketRepository)
    {
        _tournamentId = tournamentId;
        _weightCategorieId = weightCategorieId;
        _division = division;
        _bracketRepository = bracketRepository;
    }

    public async Task<bool> IsBrokenAsync() =>
        await _bracketRepository.IsExistsAsync(_tournamentId, _weightCategorieId, _division);


    public string Message => "Данная турнирная сетка уже сгенерирована для данного турнира";
}