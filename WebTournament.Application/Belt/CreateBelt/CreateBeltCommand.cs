using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Belt.CreateBelt;

public class CreateBeltCommand :  ICommand
{
    public Guid Id { get; set; }
    public int? BeltNumber { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
}