using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Trainer.GetTrainerList;

public class GetTrainerListQuery : PagedRequest, IQuery<PagedResponse<TrainerResponse[]>>
{
    
}