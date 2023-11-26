using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IFighterService
    {
        Task AddFighterAsync(FighterViewModel fighterViewModel);
        Task EditFighterAsync(FighterViewModel fighterViewModel);
        Task DeleteFighterAsync(Guid id);
        Task DeleteAllFightersAsync(Guid id);
        Task<PagedResponse<FighterViewModel[]>> FightersListAsync(PagedRequest request, Guid tournamentId);
        Task<FighterViewModel> GetFighterAsync(Guid id);
    }
}