using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Fighter.GetFighterList;

public class GetFighterListQuery : PagedRequest, IQuery<PagedResponse<FighterDto[]>>
{
    public Guid TournamentId { get; set; }
    public GetFighterListQuery(Guid tournamentId) => TournamentId = tournamentId;
}