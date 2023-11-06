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
    public class BeltService : IBeltService
    {
        public Task AddBelt(BeltViewModel beltViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<BeltViewModel[]>> BeltList(PagedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBelt(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditBelt(BeltViewModel beltViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<BeltViewModel> GetBelt(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<BeltViewModel>> GetBelts()
        {
            throw new NotImplementedException();
        }
    }
}
