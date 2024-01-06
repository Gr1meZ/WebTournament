using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Fighter.GetFighterList;

public class GetFighterListQuery : PagedRequest, IQuery<PagedResponse<FighterResponse[]>>
{
    public Guid TournamentId { get; set; }
    public GetFighterListQuery(Guid tournamentId) => TournamentId = tournamentId;
}