using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Tournament.RemoveTournament;

public class RemoveTournamentCommand : ICommand
{
    public RemoveTournamentCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}