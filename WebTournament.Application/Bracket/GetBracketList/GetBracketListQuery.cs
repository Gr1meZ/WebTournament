using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Bracket.GetBracketList;

public class GetBracketListQuery : PagedRequest, IQuery<PagedResponse<BracketResponse[]>>
{
    public GetBracketListQuery(Guid tournamentId)
    {
        TournamentId = tournamentId;
    }

    public Guid TournamentId { get; set; }
}