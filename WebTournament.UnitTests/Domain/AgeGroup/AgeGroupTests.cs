using AutoMapper;
using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Domain.Objects.AgeGroup;
using Xunit.Abstractions;

namespace WebTournament.UnitTests.Domain.AgeGroup;

public class AgeGroupTests
{
    private readonly Mock<IAgeGroupRepository> _ageGroupRepository = new();
    

    [Fact]
    public async Task AgeGroup_Must_BeCreated()
    {
        var ageGroupCommand = new CreateAgeGroupCommand()
        {
            Name = "5-6 лет",
            MaxAge = 6,
            MinAge = 5
        };
        
        _ageGroupRepository.Setup(method => method.IsExistsAsync(ageGroupCommand.MinAge, ageGroupCommand.MaxAge))
            .ReturnsAsync(false);

        var ageGroup = await BuilderAsync(Guid.NewGuid());

        Assert.Equal(ageGroup.Name, ageGroupCommand.Name);
        Assert.Equal(ageGroup.MinAge, ageGroupCommand.MinAge);
        Assert.Equal(ageGroup.MaxAge, ageGroupCommand.MaxAge);
    }
    
    [Fact]
    public async Task AgeGroup_AlreadyExists()
    {
        var ageGroupCommand = new CreateAgeGroupCommand()
        {
            Name = "5-6 лет",
            MaxAge = 6,
            MinAge = 5
        };
        
        _ageGroupRepository.Setup(method => method.IsExistsAsync(ageGroupCommand.MinAge, ageGroupCommand.MaxAge))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.AgeGroup.AgeGroup.CreateAsync(
            Guid.NewGuid(), ageGroupCommand.Name,
            ageGroupCommand.MinAge,
            ageGroupCommand.MaxAge, _ageGroupRepository.Object));
    }
    
    [Fact]
    public async Task AgeGroup_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var ageGroupCommand = new UpdateAgeGroupCommand()
        {
            Id = id,
            Name = "8-9 лет",
            MaxAge = 9,
            MinAge = 8,
        };
        
        var ageGroup = await BuilderAsync(id);

        _ageGroupRepository.Setup(method => method.GetByIdAsync(ageGroupCommand.Id))
            .ReturnsAsync(ageGroup);

        ageGroup.Change(ageGroupCommand.Name, ageGroupCommand.MinAge, ageGroupCommand.MaxAge);
        
        Assert.Equal(ageGroup.Id, ageGroupCommand.Id);
        Assert.Equal(ageGroup.MinAge, ageGroupCommand.MinAge);
        Assert.Equal(ageGroup.MaxAge, ageGroupCommand.MaxAge);
        Assert.Equal(ageGroup.Name, ageGroupCommand.Name);
    }
    
    [Fact]
    public async Task GetAgeGroup_Must_BeSuccessful()
    {
        var getAgeGroupQuery = new GetAgeGroupQuery(Guid.NewGuid());
        
        var ageGroup = await BuilderAsync(getAgeGroupQuery.Id);

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
    
    private async Task<WebTournament.Domain.Objects.AgeGroup.AgeGroup> BuilderAsync(Guid id)
    {
        _ageGroupRepository.Setup(method => method.IsExistsAsync(5, 6))
            .ReturnsAsync(false);
        
        return await WebTournament.Domain.Objects.AgeGroup.AgeGroup
            .CreateAsync(id, $"5-6 лет", 5,6,
                _ageGroupRepository.Object);
    }
}