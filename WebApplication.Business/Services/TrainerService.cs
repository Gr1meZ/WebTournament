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
    public class TrainerService : ITrainerService
    {
        public Task AddTrainer(TrainerViewModel trainerViewModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTrainer(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task EditTrainer(TrainerViewModel trainerViewModel)
        {
            throw new NotImplementedException();
        }

        public Task<TrainerViewModel> GetTrainer(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TrainerViewModel>> GetTrainers()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<TrainerViewModel[]>> TrainersList(PagedRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
