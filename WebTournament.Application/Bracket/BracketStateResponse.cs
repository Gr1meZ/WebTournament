namespace WebTournament.Application.Bracket;

public class BracketStateResponse
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public string CategorieName { get; set; }
    public List<Guid> Winners { get; set; }
}