using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.AgeGroup;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroupList;
using WebTournament.Application.AgeGroup.RemoveAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;

namespace WebTournament.IntegrationTests.AgeGroup;

public class AgeGroupTests : BaseIntegrationTest
{
    public AgeGroupTests(WebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task AgeGroup_Must_BeCreated()
    {
        var createAgeGroupCommand = new CreateAgeGroupCommand()
        {
            Name = "5-6 лет",
            MaxAge = 6,
            MinAge = 5
        };
        await Sender.Send(createAgeGroupCommand);
        
        var ageGroup = await DbContext.AgeGroups.FirstOrDefaultAsync(x =>
            x.MinAge == createAgeGroupCommand.MinAge && x.MaxAge == createAgeGroupCommand.MaxAge);
        
        Assert.NotNull(ageGroup);
    }
    
    [Fact]
    public async Task AgeGroup_AlreadyExists_ThrowsException()
    {
        var createAgeGroupCommand = new CreateAgeGroupCommand()
        {
            Name = "5-6 лет",
            MaxAge = 6,
            MinAge = 5
        };
        
        await Sender.Send(createAgeGroupCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createAgeGroupCommand));
    }
    
    [Fact]
    public async Task GetAgeGroup_IsValid()
    {
        var ageGroup = await DbContext.AgeGroups.FirstOrDefaultAsync();
        var getAgeGroupQuery = new GetAgeGroupQuery(ageGroup.Id);
        
        var ageGroupResponse = await Sender.Send(getAgeGroupQuery);
        
        Assert.NotNull(ageGroupResponse);
    }
    
    [Fact]
    public async Task GetAgeGroup_NotFound_ThrowsException()
    {
        var getAgeGroupQuery = new GetAgeGroupQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getAgeGroupQuery));
    }
    
    [Fact]
    public async Task GetAgeGroupList_Must_ReturnValidPagedResponse()
    {
        await Sender.Send(new CreateAgeGroupCommand() { MinAge = 1, MaxAge = 2, Name = "Some Age Group" });
        await Sender.Send(new CreateAgeGroupCommand() { MinAge = 8, MaxAge = 9, Name = "Some Age Group" });
        await Sender.Send(new CreateAgeGroupCommand() { MinAge = 3, MaxAge = 4, Name = "Some Age Group5" });

        var getAgeGroupListBySearch = new GetAgeGroupListQuery() { Search = "Some Age Group" };
        var getAgeGroupListByOrder = new GetAgeGroupListQuery() {OrderColumn = "minAge", OrderDir = "desc", Search = ""};

        var searchResponse = await Sender.Send(getAgeGroupListBySearch);
        var orderColumnResponse = await Sender.Send(getAgeGroupListByOrder);

        var dataBySearch = await DbContext.AgeGroups.Where(x => x.Name.Contains(getAgeGroupListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.AgeGroups.OrderByDescending(x => x.MinAge).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(ageGroup => Mapper.Map<AgeGroupResponse>(ageGroup)).ToArray();
        var expectedOrder = dataByOrder.Select(ageGroup => Mapper.Map<AgeGroupResponse>(ageGroup)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task AgeGroup_Must_BeRemoved()
    {
        var createCommand = new CreateAgeGroupCommand() { MinAge = 10, MaxAge = 11, Name = "Some Age Group" };
        
        await Sender.Send(createCommand);

        var ageGroupId =  await DbContext.AgeGroups.Where(x =>
            x.MinAge == createCommand.MinAge && x.MaxAge == createCommand.MaxAge)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveAgeGroupCommand(ageGroupId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.AgeGroups.AnyAsync(x => x.Id == ageGroupId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task AgeGroup_Must_BeUpdated()
    {
        var ageGroupId = await DbContext.AgeGroups.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateAgeGroupCommand() { Id = ageGroupId, MinAge = 12, MaxAge = 13, Name = "Some Age Group" };
        
        await Sender.Send(updateCommand);

        var updatedAgeGroup = await DbContext.AgeGroups.FindAsync(ageGroupId);
        
        Assert.Equal(updateCommand.Id, updatedAgeGroup.Id);
        Assert.Equal(updateCommand.MinAge, updatedAgeGroup.MinAge);
        Assert.Equal(updateCommand.MaxAge, updatedAgeGroup.MaxAge);
        Assert.Equal(updateCommand.Name, updatedAgeGroup.Name);
    }
}