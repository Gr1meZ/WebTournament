using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.WeightCategorie.UpdateWeightCategorie;

public class UpdateWeightCategorieCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid? AgeGroupId { get; set; }
    public int? MaxWeight { get; init; } = 0; 
    public string WeightName { get; set; }
    public string Gender { get; set; }
}