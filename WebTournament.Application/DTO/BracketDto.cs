using System.ComponentModel.DataAnnotations;

namespace WebTournament.Application.DTO;

public class BracketDto
{
    public Guid Id { get; set; }
    public Guid TournamentId { get; set; }
    [Required(ErrorMessage = "Выберите пояса")]
    public Guid[] Division { get; set; }
    public Guid AgeGroupId { get; set; }
    
    
    public string? DivisionName { get; set; }
    public string? CategoriesName { get; set; }
    public int? MaxWeight { get; set; }
}