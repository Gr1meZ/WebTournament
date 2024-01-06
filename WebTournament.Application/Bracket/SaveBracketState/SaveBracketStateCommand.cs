using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Bracket.SaveBracketState;

public class SaveBracketStateCommand : ICommand
{

    public SaveBracketStateCommand(Guid id, BracketStateRequest bracketState)
    {
        BracketState = bracketState;
        Id = id;
    }

    public Guid Id { get; }
    public BracketStateRequest BracketState;
}