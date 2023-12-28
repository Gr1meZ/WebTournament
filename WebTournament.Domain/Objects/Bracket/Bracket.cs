using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Bracket;

public class Bracket : Entity
{
    public Bracket(Guid id, Guid weightCategorieId, Guid tournamentId, Guid[] division, string state)
    {
        Id = id;
        WeightCategorieId = weightCategorieId;
        TournamentId = tournamentId;
        Division = division;
        State = state;
    }
    
    protected Bracket() {}
    
    public Guid WeightCategorieId { get; private set; }
    public Guid TournamentId { get; private set; }
    public Guid[] Division { get; private set; }
    public string State { get; private set; }
    
    public WeightCategorie.WeightCategorie WeightCategorie { get; protected set; }
    public Tournament.Tournament Tournament { get; protected set; }
    public IReadOnlyCollection<Fighter.Fighter> Fighters { get; protected set; }
}