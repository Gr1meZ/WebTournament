using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IWeightCategorieService
    {
        Task AddWeightCategorie(WeightCategorieViewModel weightCategorieViewModel);
        Task EditWeightCategorie(WeightCategorieViewModel weightCategorieViewModel);
        Task DeleteWeightCategorie(Guid id);
        Task<PagedResponse<WeightCategorieViewModel[]>> WeightCategoriesList(PagedRequest request);
        Task<WeightCategorieViewModel> GetWeightCategorie(Guid id);
        Task<List<WeightCategorieViewModel>> GetWeightCategories();
        Task<Select2Response> GetAutoCompleteWeightCategories(Select2Request request);

    }
}