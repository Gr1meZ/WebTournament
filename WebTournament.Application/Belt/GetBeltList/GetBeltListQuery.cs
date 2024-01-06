using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Belt.GetBeltList;

public class GetBeltListQuery : PagedRequest, IQuery<PagedResponse<BeltResponse[]>>
{
}