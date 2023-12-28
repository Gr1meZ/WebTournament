using WebTournament.Application.Configuration;
using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.AgeGroup.RemoveAgeGroup;

public class RemoveAgeGroupCommand : ICommand
{
    public Guid Id { get; private set; }

    public RemoveAgeGroupCommand(Guid id)
    {
        Id = id;
    }
}