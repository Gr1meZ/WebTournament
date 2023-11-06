using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Services
{
    public class AgeGroupService : IAgeGroupService
    {
        public Task AddAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<AgeGroupViewModel[]>> AgeGroupList(PagedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAgeGroup(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditAgeGroup(AgeGroupViewModel ageGroupViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroupViewModel> GetAgeGroup(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AgeGroupViewModel>> GetAgeGroups()
        {
            throw new NotImplementedException();
        }
    }
}
