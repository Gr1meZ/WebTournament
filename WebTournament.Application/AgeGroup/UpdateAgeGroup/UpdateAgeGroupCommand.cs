using WebTournament.Application.Configuration;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AgeGroup.UpdateAgeGroup;

public class UpdateAgeGroupCommand :  AgeGroupDto, ICommand
{
    public UpdateAgeGroupCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}