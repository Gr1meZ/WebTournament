using Moq;
using WebTournament.Domain.Objects.Tournament;

namespace WebTournament.UnitTests.Builders;

public class TournamentBuilder : IEntityBaseBuilder<Tournament>
{
    public static async Task<Tournament> BuildAsync(Guid id)
    {
        var tournamentRepository = new Mock<ITournamentRepository>();
        
        tournamentRepository.Setup(method => method.IsExistsAsync("Minsk Cup 2023", "Улица Победы 14"))
            .ReturnsAsync(false);
        
        return await Tournament.CreateAsync(id, "Minsk Cup 2023", DateTime.UtcNow.AddMonths(-2), "Улица Победы 14",
            tournamentRepository.Object);
    }
    public static async Task<Tournament> BuildAsync(Guid id, DateTime startDate)
    {
        var tournamentRepository = new Mock<ITournamentRepository>();
        
        tournamentRepository.Setup(method => method.IsExistsAsync("Minsk Cup 2023", "Улица Победы 14"))
            .ReturnsAsync(false);
        
        return await Tournament.CreateAsync(id, "Minsk Cup 2023", startDate, "Улица Победы 14",
            tournamentRepository.Object);
    }
}