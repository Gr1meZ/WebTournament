using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Bracket.GetBracket;

public class GetBracketQuery : IQuery<BracketStateResponse>
{
    public GetBracketQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}