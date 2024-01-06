using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Tournament.CreateTournament;

public class CreateTournamentCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public string Address { get; set; }
}