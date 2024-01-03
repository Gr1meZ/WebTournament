using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Bracket.DistributePlayers;

public class DistributePlayersCommand : ICommand
{
    public DistributePlayersCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}