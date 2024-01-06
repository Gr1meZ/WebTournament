namespace WebTournament.Application.Tournament;

public class BracketWinnerResponse
{
    public Guid Id { get; set; }
    public string TournamentName { get; set; }
    public string FirstPlayerFullName { get; set; }
    public string SecondPlayerFullName { get; set; }
    public string ThirdPlayerFullName { get; set; }
    public string FirstPlayerClubName { get; set; }
    public string SecondPlayerClubName { get; set; }
    public string ThirdPlayerClubName { get; set; }
    public string Division { get; set; }
    public string CategorieName { get; set; }
}