namespace DataAccess.Domain.Models;

public class BracketWinner
{
    public Guid Id { get; set; }
    public Guid? FirstPlaceId { get; set; }
    public Guid? SecondPlaceId { get; set; }
    public Guid? ThirdPlaceId { get; set; }
    
    public Bracket Bracket { get; set; }
    public Fighter FirstPlacePlayer { get; set; }
    public Fighter SecondPlacePlayer{ get; set; }
    public Fighter ThirdPlacePlayer { get; set; }
}