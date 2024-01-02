using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorieList;

public class GetWeightCategorieListQuery : PagedRequest, IQuery<PagedResponse<WeightCategorieDto[]>>
{
    
}