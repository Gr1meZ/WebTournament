using AutoMapper;
using Moq;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Application.Tournament;
using WebTournament.Application.Tournament.GetTournament;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Tournament;

public class TournamentTests
{
    private readonly Mock<ITournamentRepository> _tournamentRepository = new();

    [Fact]
    public async Task GetTournament_Must_BeSuccessful()
    {
        var getTournamentQuery = new GetTournamentQuery(Guid.NewGuid());
        
        var tournament = await TournamentBuilder.BuildAsync(getTournamentQuery.Id);

        _tournamentRepository.Setup(method => method.GetByIdAsync(getTournamentQuery.Id))
            .ReturnsAsync(tournament);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var tournamentResponse = mapper.Map<TournamentResponse>(tournament);
        
        Assert.Equal(tournament.Id, tournamentResponse.Id);
        Assert.Equal(tournament.Name, tournamentResponse.Name);
        Assert.Equal(tournament.StartDate, tournamentResponse.StartDate);
        Assert.Equal(tournament.Address, tournamentResponse.Address);
        
    }
    
}