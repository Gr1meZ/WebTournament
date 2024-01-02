using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Belt.RemoveBelt;

public class RemoveBeltCommand : ICommand
{
    public RemoveBeltCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}