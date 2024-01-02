using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Trainer.GetTrainerList;

public class GetTrainerListQuery : PagedRequest, IQuery<PagedResponse<TrainerDto[]>>
{
    
}