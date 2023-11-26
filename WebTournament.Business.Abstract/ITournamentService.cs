using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface ITournamentService
    {
        Task AddTournamentAsync(TournamentViewModel tournamentViewModel);
        Task EditTournamentAsync(TournamentViewModel tournamentViewModel);
        Task DeleteTournamentAsync(Guid id);
        Task<PagedResponse<TournamentViewModel[]>> TournamentsListAsync(PagedRequest request);
        Task<TournamentViewModel> GetTournamentAsync(Guid id);
        Task<Select2Response> GetSelect2TournamentsAsync(Select2Request request);
        Task<List<BracketWinnerViewModel>> GetTournamentResultsAsync(Guid tournamentId);
    }
}