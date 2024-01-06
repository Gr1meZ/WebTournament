using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.AgeGroup.RemoveAgeGroup;

public class RemoveAgeGroupCommand : ICommand
{
    public Guid Id { get; }

    public RemoveAgeGroupCommand(Guid id)
    {
        Id = id;
    }
}