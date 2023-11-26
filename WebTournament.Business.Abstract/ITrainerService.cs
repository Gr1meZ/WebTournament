using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract
{
    public interface ITrainerService
    {
        Task AddTrainerAsync(TrainerViewModel trainerViewModel);
        Task EditTrainerAsync(TrainerViewModel trainerViewModel);
        Task DeleteTrainerAsync(Guid id);
        Task<PagedResponse<TrainerViewModel[]>> TrainersListAsync(PagedRequest request);
        Task<TrainerViewModel> GetTrainerAsync(Guid id);
        Task<Select2Response> GetAutoCompleteTrainersAsync(Select2Request request);


    }
}