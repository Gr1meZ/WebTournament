using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Club.CreateClub;

public class CreateClubCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}