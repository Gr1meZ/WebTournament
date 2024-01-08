using AutoMapper;
using Moq;
using WebTournament.Application.Belt;
using WebTournament.Application.Belt.GetBelt;
using WebTournament.Application.Club;
using WebTournament.Application.Club.GetClub;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Domain.Objects.Club;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Club;

public class ClubTests
{
    private readonly Mock<IClubRepository> _clubRepository = new();

    [Fact]
    public async Task GetClub_Must_BeSuccessful()
    {
        var getBeltQuery = new GetClubQuery(Guid.NewGuid());
        
        var belt = await ClubBuilder.BuildAsync(getBeltQuery.Id);

        _clubRepository.Setup(method => method.GetByIdAsync(getBeltQuery.Id))
            .ReturnsAsync(belt);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var clubResponse = mapper.Map<ClubResponse>(belt);
        
        Assert.Equal(belt.Id, clubResponse.Id);
        Assert.Equal(belt.Name, clubResponse.Name);
    }
}