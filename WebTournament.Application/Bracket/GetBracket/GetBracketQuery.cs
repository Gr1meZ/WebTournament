using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Bracket.GetBracket;

public class GetBracketQuery : IQuery<BracketState>
{
    public GetBracketQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}