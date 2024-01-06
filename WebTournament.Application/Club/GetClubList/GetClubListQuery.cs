using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Club.GetClubList;

public class GetClubListQuery : PagedRequest, IQuery<PagedResponse<ClubResponse[]>>
{
    
}