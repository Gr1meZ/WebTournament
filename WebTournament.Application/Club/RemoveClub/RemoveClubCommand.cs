using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Club.RemoveClub;

public class RemoveClubCommand : ICommand
{
    public RemoveClubCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}