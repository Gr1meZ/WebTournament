using Moq;
using WebTournament.Domain.Objects.Club;

namespace WebTournament.UnitTests.Builders;

public class ClubBuilder : IEntityBaseBuilder<Club>
{
    public static async Task<Club> BuildAsync(Guid id)
    {
        var clubRepository = new Mock<IClubRepository>();
        
        clubRepository.Setup(method => method.IsExistsAsync("Юнош"))
            .ReturnsAsync(false);
        
        return await Club.CreateAsync(id, "Юнош", clubRepository.Object);
    }
}