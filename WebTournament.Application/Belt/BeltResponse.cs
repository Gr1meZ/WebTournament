namespace WebTournament.Application.Belt;

public class BeltResponse
{
    public Guid Id { get; set; }
    public int? BeltNumber { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
}