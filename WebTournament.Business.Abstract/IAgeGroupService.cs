using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IAgeGroupService
    {
        Task AddAgeGroup(AgeGroupViewModel ageGroupViewModel);
        Task EditAgeGroup(AgeGroupViewModel ageGroupViewModel);
        Task DeleteAgeGroup(Guid id);
        Task<PagedResponse<AgeGroupViewModel[]>> AgeGroupList(PagedRequest request);
        Task<AgeGroupViewModel> GetAgeGroup(Guid id);
        Task<List<AgeGroupViewModel>> GetAgeGroups();
    }
}