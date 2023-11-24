using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface IFighterService
    {
        Task AddFighter(FighterViewModel fighterViewModel);
        Task EditFighter(FighterViewModel fighterViewModel);
        Task DeleteFighter(Guid id);
        Task DeleteAllFighters(Guid id);
        Task<PagedResponse<FighterViewModel[]>> FightersList(PagedRequest request, Guid tournamentId);
        Task<FighterViewModel> GetFighter(Guid id);
        Task<List<FighterViewModel>> GetFighters();
    }
}