using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Bracket.Validators;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Bracket;

public class BracketTests
{
    private readonly Mock<IBracketRepository> _bracketRepository = new();

    [Fact]
    public async Task Fighter_DontHaveBracket_ThrowsException()
    {
        var bracketId = Guid.Empty;
        var fighter = await FighterBuilder.BuildAsync(Guid.NewGuid());
        Assert.Throws<ValidationException>(() =>
            BracketValidator.IsFighterHaveBracket(bracketId, fighter.Surname, fighter.Name));
    }
    
    [Fact]
    public void AgeGroup_IsNotSelected_ThrowsException()
    {
        var ageGroupId = Guid.Empty;
        Assert.Throws<ValidationException>(() =>
            BracketValidator.ValidateAgeGroup(ageGroupId));
    }
}