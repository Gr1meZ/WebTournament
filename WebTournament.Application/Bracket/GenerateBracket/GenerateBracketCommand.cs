using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Bracket.GenerateBracket;

public class GenerateBracketCommand :  ICommand
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    public Guid[] Division { get; set; }
    public Guid AgeGroupId { get; set; }
    
    public string? DivisionName { get; set; }
    public string? CategoriesName { get; set; }
    public int? MaxWeight { get; set; }
}