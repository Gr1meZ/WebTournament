namespace WebTournament.Models;

public class BracketModel
{
    public Guid WeightCategorieId { get; set; }
    public Guid TournamentId { get; set; }
    public Guid[] Division { get; set; }
    public string State { get; set; }
}