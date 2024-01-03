using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Fighter.RemoveAllFighters;

public class RemoveAllFightersCommand : ICommand
{
    public RemoveAllFightersCommand(Guid tournamentId)
    {
        Id = tournamentId;
    }

    public Guid Id { get; }
}