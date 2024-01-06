using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.AgeGroup.CreateAgeGroup;

public class CreateAgeGroupCommand :  ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int? MinAge { get; set;}
    public int? MaxAge { get; set; }
}