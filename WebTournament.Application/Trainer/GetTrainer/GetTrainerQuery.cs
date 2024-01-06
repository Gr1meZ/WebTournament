using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Trainer.GetTrainer;

public class GetTrainerQuery : IQuery<TrainerResponse>
{
    public Guid Id { get; private set; }
    public GetTrainerQuery(Guid id) => Id = id;
}