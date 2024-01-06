using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Club.UpdateClub;

public class UpdateClubCommand :  ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}