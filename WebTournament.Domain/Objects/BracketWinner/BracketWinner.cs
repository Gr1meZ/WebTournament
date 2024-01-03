using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.BracketWinner;

public class BracketWinner : Entity
{
    private BracketWinner(Guid id, Guid? firstPlaceId, Guid? secondPlaceId, Guid? thirdPlaceId)
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
    
    public Bracket.Bracket Bracket { get; protected set; }
    public Fighter.Fighter FirstPlacePlayer { get; protected set; }
    public Fighter.Fighter SecondPlacePlayer{ get; protected set; } 
    public Fighter.Fighter ThirdPlacePlayer { get; protected set; }

    public static BracketWinner Create(Guid id, Guid? firstPlaceId, Guid? secondPlaceId, Guid? thirdPlaceId)
    {
        return new BracketWinner(id, firstPlaceId, secondPlaceId, thirdPlaceId);
    }
    public void Change(Guid? firstPlaceId, Guid? secondPlaceId, Guid? thirdPlaceId)
    {
        FirstPlaceId = firstPlaceId;
        SecondPlaceId = secondPlaceId;
        ThirdPlaceId = thirdPlaceId;
    }
}