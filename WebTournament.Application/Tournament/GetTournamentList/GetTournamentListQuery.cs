using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Tournament.GetTournamentList;

public class GetTournamentListQuery : PagedRequest, IQuery<PagedResponse<TournamentDto[]>>
{
    
}