using AutoMapper;
using Moq;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.AgeGroup;

public class AgeGroupTests
{
    private readonly Mock<IAgeGroupRepository> _ageGroupRepository = new();

    [Fact]
    public async Task GetAgeGroup_Must_BeSuccessful()
    {
        var getAgeGroupQuery = new GetAgeGroupQuery(Guid.NewGuid());
        
        var ageGroup = await AgeGroupBuilder.BuildAsync(getAgeGroupQuery.Id);

        _ageGroupRepository.Setup(method => method.GetByIdAsync(getAgeGroupQuery.Id))
            .ReturnsAsync(ageGroup);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var ageGroupResponse = mapper.Map<AgeGroupResponse>(ageGroup);
        
        Assert.Equal(ageGroup.Id, ageGroupResponse.Id);
        Assert.Equal(ageGroup.MinAge, ageGroupResponse.MinAge);
        Assert.Equal(ageGroup.MaxAge, ageGroupResponse.MaxAge);
        Assert.Equal(ageGroup.Name, ageGroupResponse.Name);
    }
}