using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public class GetAgeGroupListQuery : PagedRequest, IQuery<PagedResponse<AgeGroupDto[]>>
{
    
}