using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public class GetAgeGroupListQuery : PagedRequest, IQuery<PagedResponse<AgeGroupResponse[]>>
{
    
}