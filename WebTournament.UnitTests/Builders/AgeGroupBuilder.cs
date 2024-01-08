using Moq;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.UnitTests.Builders;

public class AgeGroupBuilder : IEntityBaseBuilder<AgeGroup>
{
    public static async Task<AgeGroup> BuildAsync(Guid id)
    {
        var ageGroupRepository = new Mock<IAgeGroupRepository>();
        
        ageGroupRepository.Setup(method => method.IsExistsAsync(5, 6))
            .ReturnsAsync(false);
        
        return await AgeGroup.CreateAsync(id, $"5-6 лет", 5,6, ageGroupRepository.Object);
    }
}