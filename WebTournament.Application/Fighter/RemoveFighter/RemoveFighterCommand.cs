using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Fighter.RemoveFighter;

public class RemoveFighterCommand : ICommand
{
    public RemoveFighterCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}