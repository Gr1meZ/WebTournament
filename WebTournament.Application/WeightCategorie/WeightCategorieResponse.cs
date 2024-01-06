namespace WebTournament.Application.WeightCategorie;

public class WeightCategorieResponse
{
    public Guid Id { get; set; }
    public Guid? AgeGroupId { get; set; }
    public string AgeGroupName { get; init; } = string.Empty;
    public int? MaxWeight { get; init; } = 0; 
    public string WeightName { get; set; }
    public string Gender { get; set; }
}