using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IAgeGroupService
    {
        Task AddAgeGroupAsync(AgeGroupViewModel ageGroupViewModel);
        Task EditAgeGroupAsync(AgeGroupViewModel ageGroupViewModel);
        Task DeleteAgeGroupAsync(Guid id);
        Task<PagedResponse<AgeGroupViewModel[]>> AgeGroupListAsync(PagedRequest request);
        Task<AgeGroupViewModel> GetAgeGroupAsync(Guid id);
        Task<Select2Response> GetSelect2AgeGroupsAsync(Select2Request request);

    }
}