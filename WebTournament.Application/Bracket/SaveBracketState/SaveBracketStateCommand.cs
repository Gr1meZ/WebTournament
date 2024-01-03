using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Bracket.SaveBracketState;

public class SaveBracketStateCommand : ICommand
{

    public SaveBracketStateCommand(Guid id, BracketState bracketState)
    {
        BracketState = bracketState;
        Id = id;
    }

    public Guid Id { get; }
    public BracketState BracketState;
}