using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface ITournamentService
    {
        Task AddTournament(TournamentViewModel tournamentViewModel);
        Task EditTournament(TournamentViewModel tournamentViewModel);
        Task DeleteTournament(Guid id);
        Task<PagedResponse<TournamentViewModel[]>> TournamentsList(PagedRequest request);
        Task<TournamentViewModel> GetTournament(Guid id);
        Task<List<TournamentViewModel>> GetTournaments();
    }
}