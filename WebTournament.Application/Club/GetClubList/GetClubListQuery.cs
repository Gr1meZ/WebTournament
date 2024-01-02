using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Club.GetClubList;

public class GetClubListQuery : PagedRequest, IQuery<PagedResponse<ClubDto[]>>
{
    
}