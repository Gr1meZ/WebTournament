using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IClubService
    {
        Task AddClub(ClubViewModel clubViewModel);
        Task EditClub(ClubViewModel clubViewModel);
        Task DeleteClub(Guid id);
        Task<PagedResponse<ClubViewModel[]>> ClubList(PagedRequest request);
        Task<ClubViewModel> GetClub(Guid id);
        Task<List<ClubViewModel>> GetClubs();
        Task<Select2Response> GetAutoCompleteClubs(Select2Request request);

    }
}