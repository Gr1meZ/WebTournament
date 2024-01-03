using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Bracket.RemoveBracket;

public class RemoveBracketCommand : ICommand
{
    public RemoveBracketCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}