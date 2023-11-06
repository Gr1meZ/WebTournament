using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IBeltService
    {
        Task AddBelt(BeltViewModel beltViewModel);
        Task EditBelt(BeltViewModel beltViewModel);
        Task DeleteBelt(Guid id);
        Task<PagedResponse<BeltViewModel[]>> BeltList(PagedRequest request);
        Task<BeltViewModel> GetBelt(Guid id);
        Task<List<BeltViewModel>> GetBelts();
    }
}