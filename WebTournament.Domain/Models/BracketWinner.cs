using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Models;

public class BracketWinner : Entity
{
    public BracketWinner(Guid id, Guid? firstPlaceId, Guid? secondPlaceId, Guid? thirdPlaceId, Bracket bracket)
    {
        Id = id;
        FirstPlaceId = firstPlaceId;
        SecondPlaceId = secondPlaceId;
        ThirdPlaceId = thirdPlaceId;
    }
    protected BracketWinner() {}
    
    public Guid? FirstPlaceId { get; private set; }
    public Guid? SecondPlaceId { get; private set; }
    public Guid? ThirdPlaceId { get; private set; }
    
    public Bracket Bracket { get; protected set; }
    public Fighter FirstPlacePlayer { get; protected set; }
    public Fighter SecondPlacePlayer{ get; protected set; } 
    public Fighter ThirdPlacePlayer { get; protected set; }
}