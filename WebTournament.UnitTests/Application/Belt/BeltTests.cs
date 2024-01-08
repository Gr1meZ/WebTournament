using AutoMapper;
using Moq;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.Belt;
using WebTournament.Application.Belt.GetBelt;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Domain.Objects.Belt;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Belt;

public class BeltTests
{
    private readonly Mock<IBeltRepository> _beltRepository = new();

    [Fact]
    public async Task GetAgeGroup_Must_BeSuccessful()
    {
        var getBeltQuery = new GetBeltQuery(Guid.NewGuid());
        
        var belt = await BeltBuilder.BuildAsync(getBeltQuery.Id);

        _beltRepository.Setup(method => method.GetByIdAsync(getBeltQuery.Id))
            .ReturnsAsync(belt);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var beltResponse = mapper.Map<BeltResponse>(belt);
        
        Assert.Equal(belt.Id, beltResponse.Id);
        Assert.Equal(belt.BeltNumber, beltResponse.BeltNumber);
        Assert.Equal(belt.ShortName, beltResponse.ShortName);
        Assert.Equal(belt.FullName, beltResponse.FullName);
    }
}