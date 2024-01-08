using Moq;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.Objects.Trainer;

namespace WebTournament.UnitTests.Builders;

public class TrainerBuilder : IEntityBaseBuilder<Trainer>
{
    public static async Task<Trainer> BuildAsync(Guid id)
    {
        var trainerRepository = new Mock<ITrainerRepository>();
        
        trainerRepository.Setup(method => method.IsExistsAsync("Иван", "Иванов", "Иванович", "+375253333333", Guid.NewGuid()))
            .ReturnsAsync(false);
        
        return await Trainer.CreateAsync(id, "Иван", "Иванов", "Иванович", "+375253333333", Guid.NewGuid(),
            trainerRepository.Object);
    }
    public static async Task<Trainer> BuildAsync(Guid id, Guid clubId)
    {
        var trainerRepository = new Mock<ITrainerRepository>();
        
        trainerRepository.Setup(method => method.IsExistsAsync("Иван", "Иванов", "Иванович", "+375253333333", Guid.NewGuid()))
            .ReturnsAsync(false);
        
        return await Trainer.CreateAsync(id, "Иван", "Иванов", "Иванович", "+375253333333", clubId,
            trainerRepository.Object);
    }
}