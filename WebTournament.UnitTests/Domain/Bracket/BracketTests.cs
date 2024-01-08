using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Bracket;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Bracket.SaveBracketState;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Bracket;

public class BracketTests
{
    private readonly Mock<IBracketRepository> _bracketRepository = new();
    
    [Fact]
    public async Task Bracket_Must_BeCreated()
    {
        var generateBracketCommand = new GenerateBracketCommand()
        {
            Division = new [] {Guid.NewGuid()},
            TournamentId = Guid.NewGuid(),
            DivisionName = "1 гып",
            MaxWeight = 10,
            AgeGroupId = Guid.NewGuid(),
            CategoriesName = "5-6 лет 10кг"
        };
        
        var weightCategorieId = Guid.NewGuid();
        
        _bracketRepository.Setup(method => method
                .IsExistsAsync(generateBracketCommand.TournamentId, weightCategorieId, generateBracketCommand.Division))
            .ReturnsAsync(false);

        var bracket = await BracketBuilder.BuildAsync(Guid.NewGuid(), generateBracketCommand.TournamentId, weightCategorieId, generateBracketCommand.Division);
        
        Assert.Equal(bracket.TournamentId, generateBracketCommand.TournamentId);
        Assert.Equal(bracket.WeightCategorieId, weightCategorieId);
        Assert.Equal(bracket.Division, generateBracketCommand.Division);
    }
    
    [Fact]
    public async Task Bracket_AlreadyExists()
    {
        var generateBracketCommand = new GenerateBracketCommand()
        {
            Division = new [] {Guid.NewGuid()},
            TournamentId = Guid.NewGuid(),
            DivisionName = "1 гып",
            MaxWeight = 10,
            AgeGroupId = Guid.NewGuid(),
            CategoriesName = "5-6 лет 10кг"
        };
        
        var weightCategorieId = Guid.NewGuid();
        
        _bracketRepository.Setup(method => method
                .IsExistsAsync(generateBracketCommand.TournamentId, weightCategorieId, generateBracketCommand.Division))
            .ReturnsAsync(true);
        
        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Bracket.Bracket.CreateAsync(
            Guid.NewGuid() ,weightCategorieId, generateBracketCommand.TournamentId, generateBracketCommand.Division, string.Empty, _bracketRepository.Object));
    }
    
    [Fact]
    public async Task BracketState_Must_BeUpdated()
    {
        var saveBracketStateCommand = new SaveBracketStateCommand(Guid.NewGuid(), new BracketStateRequest(){State = "Some state"});
        
        var bracket = await BracketBuilder.BuildAsync(Guid.NewGuid());
        
        _bracketRepository.Setup(method => method.GetByIdAsync(saveBracketStateCommand.Id))
            .ReturnsAsync(bracket);

        bracket.UpdateState(saveBracketStateCommand.BracketState.State);
        
        Assert.Equal(bracket.State, saveBracketStateCommand.BracketState.State);
    }
}