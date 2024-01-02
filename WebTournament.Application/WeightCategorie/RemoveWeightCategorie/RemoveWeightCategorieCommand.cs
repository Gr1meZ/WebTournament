using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.WeightCategorie.RemoveWeightCategorie;

public class RemoveWeightCategorieCommand : ICommand
{
    public RemoveWeightCategorieCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}