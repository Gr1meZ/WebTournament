using Moq;
using WebTournament.Domain.Objects.BracketWinner;

namespace WebTournament.UnitTests.Domain.BracketWinner;

public class BracketWinnerTests
{
    [Fact]
    public void BracketWinner_Must_BeCreated()
    {
        var id = Guid.NewGuid();
        var firstPlayerId = Guid.NewGuid();
        var secondPlayerId = Guid.NewGuid();
        var thirdPlayerId = Guid.NewGuid();

        var bracketWinner = WebTournament.Domain.Objects.BracketWinner.BracketWinner.Create(id, firstPlayerId, secondPlayerId,
            thirdPlayerId);
        
        Assert.Equal(bracketWinner.Id, id);
        Assert.Equal(bracketWinner.FirstPlaceId, firstPlayerId);
        Assert.Equal(bracketWinner.SecondPlaceId, secondPlayerId);
        Assert.Equal(bracketWinner.ThirdPlaceId, thirdPlayerId);

    }
    
    [Fact]
    public void BracketWinner_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        var firstPlayerId = Guid.NewGuid();
        var secondPlayerId = Guid.NewGuid();
        var thirdPlayerId = Guid.NewGuid();
        var bracketWinner = WebTournament.Domain.Objects.BracketWinner.BracketWinner.Create(id, Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid());
        
        bracketWinner.Change(firstPlayerId, secondPlayerId, thirdPlayerId);
        
        Assert.Equal(bracketWinner.Id, id);
        Assert.Equal(bracketWinner.FirstPlaceId, firstPlayerId);
        Assert.Equal(bracketWinner.SecondPlaceId, secondPlayerId);
        Assert.Equal(bracketWinner.ThirdPlaceId, thirdPlayerId);
    }
}