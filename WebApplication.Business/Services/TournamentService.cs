using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class TournamentService : ITournamentService
    {
        public Task AddTournament(TournamentViewModel tournamentViewModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTournament(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditTournament(TournamentViewModel tournamentViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<TournamentViewModel> GetTournament(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TournamentViewModel>> GetTournaments()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<TournamentViewModel[]>> TournamentsList(PagedRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
