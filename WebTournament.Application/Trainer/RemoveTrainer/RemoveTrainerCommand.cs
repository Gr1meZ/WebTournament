using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Trainer.RemoveTrainer;

public class RemoveTrainerCommand : ICommand
{
    public RemoveTrainerCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}