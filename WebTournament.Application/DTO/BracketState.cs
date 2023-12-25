namespace WebTournament.Application.DTO;

public class BracketState
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public string CategorieName { get; set; }
    public List<Guid> Winners { get; set; }
}