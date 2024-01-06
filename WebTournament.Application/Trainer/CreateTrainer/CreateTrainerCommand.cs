using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Trainer.CreateTrainer;

public class CreateTrainerCommand : ICommand
{
    public Guid Id { get; set; }

    public Guid? ClubId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }

    public string Phone { get; set; }
}