namespace WebTournament.Application.Bracket.DistributePlayers;

public class BracketData
{
    public List<List<Team>> teams { get; set; }
    public List<List<List<List<int?>>>> results { get; set; }

}