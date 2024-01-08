using Moq;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.UnitTests.Builders;

public class WeightCategorieBuilder : IEntityBaseBuilder<WeightCategorie>
{
    public static async Task<WeightCategorie> BuildAsync(Guid id)
    {
        var weightCategorieRepository = new Mock<IWeightCategorieRepository>();

        weightCategorieRepository.Setup(method =>
                method.IsExistsAsync(10, Gender.Male.MapToString(), Guid.NewGuid()))
            .ReturnsAsync(false);

        return await WeightCategorie.CreateAsync(id, Guid.NewGuid(), 10, "10kg", Gender.Male, weightCategorieRepository.Object);
    }
    public static async Task<WeightCategorie> BuildAsync(Guid id, Guid ageGroupId)
    {
        var weightCategorieRepository = new Mock<IWeightCategorieRepository>();

        weightCategorieRepository.Setup(method =>
                method.IsExistsAsync(10, Gender.Male.MapToString(), Guid.NewGuid()))
            .ReturnsAsync(false);

        return await WeightCategorie.CreateAsync(id, ageGroupId, 10, "10kg", Gender.Male, weightCategorieRepository.Object);
    }
}