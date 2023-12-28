using WebTournament.Application.Common;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public class AgeGroupListResponse : PagedResponse<AgeGroupDto>
{
    public AgeGroupListResponse(AgeGroupDto items, int totalItemCount, int pageNumber, int pageSize) : base(items, totalItemCount, pageNumber, pageSize)
    {
    }
}