namespace WebTournament.Models;

public class BracketViewModel
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public Guid[] Division { get; set; }
    public Guid AgeGroupId { get; set; }
    
    
    public string? DivisionName { get; set; }
    public string? CategoriesName { get; set; }
    public int? MaxWeight { get; set; }
}