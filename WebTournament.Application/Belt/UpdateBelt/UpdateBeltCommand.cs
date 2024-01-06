using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Belt.UpdateBelt;

public class UpdateBeltCommand : ICommand
{
    public Guid Id { get; set; }
    public int? BeltNumber { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
}