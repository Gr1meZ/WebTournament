using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorieList;

public class GetWeightCategorieListQuery : PagedRequest, IQuery<PagedResponse<WeightCategorieResponse[]>>
{
    
}