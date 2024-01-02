using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Trainer.GetTrainer;

public class GetTrainerQuery : IQuery<TrainerDto>
{
    public Guid Id { get; private set; }
    public GetTrainerQuery(Guid id) => Id = id;
}