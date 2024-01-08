using Moq;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.Fighter;

namespace WebTournament.UnitTests.Builders;

public class FighterBuilder : IEntityBaseBuilder<Fighter>
{
    public static async Task<Fighter> BuildAsync(Guid id, Guid weightCategorieId, Guid beltId, Guid trainerId, DateTime birthDate, Guid tournamentId)
    {
        var fighterRepository = new Mock<IFighterRepository>();
        
        fighterRepository.Setup(method => method.IsExistsAsync("Иванов", "Иван", "Москва", tournamentId))
            .ReturnsAsync(false);
        
        return await Fighter.CreateAsync(id, tournamentId, weightCategorieId, beltId,trainerId,
            null, "Иван", "Иванов", birthDate, "Россия", "Москва", Gender.Male.MapToString(), 
            fighterRepository.Object);
    }

    public static async Task<Fighter> BuildAsync(Guid id)
    {
        var fighterRepository = new Mock<IFighterRepository>();
        var tournamentId = Guid.NewGuid();
        
        fighterRepository.Setup(method => method.IsExistsAsync("Иванов", "Иван", "Москва", tournamentId))
            .ReturnsAsync(false);
        
        return await Fighter.CreateAsync(id, tournamentId, Guid.NewGuid(),Guid.NewGuid(),
            Guid.NewGuid(), null,"Иван", "Иванов", DateTime.UtcNow.AddYears(-10), "Россия", "Москва", Gender.Male.MapToString(), 
            fighterRepository.Object);
    }
}