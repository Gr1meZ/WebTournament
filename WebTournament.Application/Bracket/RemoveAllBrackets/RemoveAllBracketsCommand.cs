using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Bracket.RemoveAllBrackets;

public class RemoveAllBracketsCommand : ICommand
{
    public RemoveAllBracketsCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}