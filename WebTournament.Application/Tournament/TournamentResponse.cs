namespace WebTournament.Application.Tournament;

public class TournamentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public string Address { get; set; }
}