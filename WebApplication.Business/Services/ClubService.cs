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
    public class ClubService : IClubService
    {
        public Task AddClub(ClubViewModel clubViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<ClubViewModel[]>> ClubList(PagedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClub(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditClub(ClubViewModel clubViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<ClubViewModel> GetClub(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ClubViewModel>> GetClubs()
        {
            throw new NotImplementedException();
        }
    }
}
