namespace WebTournament.Models;

public class BracketState
{
    public Guid Id { get; set; }
    public string State { get; set; }

    public List<Guid> Winners { get; set; }
}