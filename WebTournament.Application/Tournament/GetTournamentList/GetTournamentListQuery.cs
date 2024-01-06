using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Tournament.GetTournamentList;

public class GetTournamentListQuery : PagedRequest, IQuery<PagedResponse<TournamentResponse[]>>
{
    
}