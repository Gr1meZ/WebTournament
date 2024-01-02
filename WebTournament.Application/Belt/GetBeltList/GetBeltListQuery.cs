using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Belt.GetBeltList;

public class GetBeltListQuery : PagedRequest, IQuery<PagedResponse<BeltDto[]>>
{
    
}