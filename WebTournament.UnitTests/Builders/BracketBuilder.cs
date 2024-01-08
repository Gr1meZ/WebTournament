using Moq;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.Club;

namespace WebTournament.UnitTests.Builders;

public class BracketBuilder : IEntityBaseBuilder<Bracket>
{
    public static async Task<Bracket> BuildAsync(Guid id)
    {
        var bracketRepository = new Mock<IBracketRepository>();
        
        bracketRepository.Setup(method => method
                .IsExistsAsync(Guid.NewGuid(), Guid.NewGuid(), new []{Guid.NewGuid()}))
            .ReturnsAsync(false);
        
        return await Bracket.CreateAsync(id,  Guid.NewGuid(),Guid.NewGuid(),new [] {Guid.NewGuid()}, string.Empty,  bracketRepository.Object);
    }
    
    public static async Task<Bracket> BuildAsync(Guid id, Guid tournamentId, Guid weightCategorieId, Guid[] division)
    {
        var bracketRepository = new Mock<IBracketRepository>();
        
        bracketRepository.Setup(method => method
                .IsExistsAsync(Guid.NewGuid(), Guid.NewGuid(), new []{Guid.NewGuid()}))
            .ReturnsAsync(false);
        
        return await Bracket.CreateAsync(id,  weightCategorieId,tournamentId,division, string.Empty,  bracketRepository.Object);
    }
}