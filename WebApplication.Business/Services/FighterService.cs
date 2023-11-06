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
    public class FighterService : IFighterService
    {
        public Task AddFighter(FighterViewModel fighterViewModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFighter(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditFighter(FighterViewModel fighterViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<FighterViewModel[]>> FightersList(PagedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<FighterViewModel> GetFighter(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FighterViewModel>> GetFighters()
        {
            throw new NotImplementedException();
        }
    }
}
