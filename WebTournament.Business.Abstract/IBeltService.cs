using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IBeltService
    {
        Task AddBeltAsync(BeltViewModel beltViewModel);
        Task EditBeltAsync(BeltViewModel beltViewModel);
        Task DeleteBeltAsync(Guid id);
        Task<PagedResponse<BeltViewModel[]>> BeltListAsync(PagedRequest request);
        Task<BeltViewModel> GetBeltAsync(Guid id);
        Task<Select2Response> GetSelect2BeltsAsync(Select2Request request);

    }
}