using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IClubService
    {
        Task AddClubAsync(ClubViewModel clubViewModel);
        Task EditClubAsync(ClubViewModel clubViewModel);
        Task DeleteClubAsync(Guid id);
        Task<PagedResponse<ClubViewModel[]>> ClubListAsync(PagedRequest request);
        Task<ClubViewModel> GetClubAsync(Guid id);
        Task<Select2Response> GetSelect2ClubsAsync(Select2Request request);

    }
}