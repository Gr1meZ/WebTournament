namespace WebTournament.Application.Bracket;

public class BracketStateRequest
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public List<Guid> Winners { get; set; }

}