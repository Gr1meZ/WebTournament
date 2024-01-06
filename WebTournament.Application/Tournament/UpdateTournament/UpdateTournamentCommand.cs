using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Tournament.UpdateTournament;

public class UpdateTournamentCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public string Address { get; set; }
}