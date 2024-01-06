using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Fighter.UpdateFighter;

public class UpdateFighterCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid? TournamentId { get; set; }
    public Guid? WeightCategorieId { get; set; }
    public Guid? BeltId { get; set; }
    public Guid? TrainerId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public int Age { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Gender { get; set; }
    public string TournamentName { get; init; } = string.Empty;
    public string WeightCategorieName { get; init; } = string.Empty;
    public int WeightNumber { get; set; } 
    public string? BeltShortName { get; set; } 
    public int BeltNumber { get; set; }
    public string? TrainerName { get; set; } 
    public string? ClubName { get; set; } 
}