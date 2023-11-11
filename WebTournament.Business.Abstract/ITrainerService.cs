using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface ITrainerService
    {
        Task AddTrainer(TrainerViewModel trainerViewModel);
        Task EditTrainer(TrainerViewModel trainerViewModel);
        Task DeleteTrainer(Guid id);
        Task<PagedResponse<TrainerViewModel[]>> TrainersList(PagedRequest request);
        Task<TrainerViewModel> GetTrainer(Guid id);
        Task<List<TrainerViewModel>> GetTrainers();
        Task<Select2Response> GetAutoCompleteTrainers(Select2Request request);


    }
}