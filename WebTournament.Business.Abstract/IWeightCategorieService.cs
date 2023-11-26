using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IWeightCategorieService
    {
        Task AddWeightCategorieAsync(WeightCategorieViewModel weightCategorieViewModel);
        Task EditWeightCategorieAsync(WeightCategorieViewModel weightCategorieViewModel);
        Task DeleteWeightCategorieAsync(Guid id);
        Task<PagedResponse<WeightCategorieViewModel[]>> WeightCategoriesListAsync(PagedRequest request);
        Task<WeightCategorieViewModel> GetWeightCategorieAsync(Guid id);
        Task<Select2Response> GetSelect2WeightCategoriesAsync(Select2Request request);

    }
}