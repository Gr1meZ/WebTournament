using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Bracket.GetBracketList;

public class GetBracketListQuery : PagedRequest, IQuery<PagedResponse<BracketDto[]>>
{
    public GetBracketListQuery(Guid tournamentId)
    {
        TournamentId = tournamentId;
    }

    public Guid TournamentId { get; set; }
}