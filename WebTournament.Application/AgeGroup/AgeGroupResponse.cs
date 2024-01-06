namespace WebTournament.Application.AgeGroup;

public class AgeGroupResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int? MinAge { get; set;}
    public int? MaxAge { get; set; }
}