using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IAgeGroupService
    {
        Task AddAgeGroup(AgeGroupViewModel ageGroupViewModel);
        Task EditProductType(AgeGroupViewModel ageGroupViewModel);
        Task DeleteProductType(Guid id);
        Task<PagedResponse<AgeGroupViewModel[]>> ProductTypeList(PagedRequest request);
        Task<AgeGroupViewModel> GetProductType(Guid id);
        Task<List<AgeGroupViewModel>> GetProductTypes();
    }
}