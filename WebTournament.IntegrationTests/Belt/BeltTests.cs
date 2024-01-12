using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Belt;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.GetBelt;
using WebTournament.Application.Belt.GetBeltList;
using WebTournament.Application.Belt.RemoveBelt;
using WebTournament.Application.Belt.UpdateBelt;

namespace WebTournament.IntegrationTests.Belt;

public class BeltTests : BaseIntegrationTest
{
    public BeltTests(WebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Belt_Must_BeCreated()
    {
        var createBeltCommand = CreateBeltCommand();
        
        await Sender.Send(createBeltCommand);
        
        var belt = await DbContext.Belts.FirstOrDefaultAsync(x =>
            x.ShortName == createBeltCommand.ShortName && x.BeltNumber == createBeltCommand.BeltNumber);
        
        Assert.NotNull(belt);
    }
    
    [Fact]
    public async Task Belt_AlreadyExists_ThrowsException()
    {
        var createBeltCommand = CreateBeltCommand();
        
        await Sender.Send(createBeltCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createBeltCommand));
    }
    
    [Fact]
    public async Task GetBelt_IsValid()
    {
        var beltId = await DbContext.Belts.Select(x => x.Id).FirstOrDefaultAsync();
        var getBeltQuery = new GetBeltQuery(beltId);
        
        var beltResponse = await Sender.Send(getBeltQuery);
        
        Assert.NotNull(beltResponse);
    }
    
    [Fact]
    public async Task GetBelt_NotFound_ThrowsException()
    {
        var getBeltQuery = new GetBeltQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getBeltQuery));
    }
    
    [Fact]
    public async Task GetBeltList_Must_ReturnValidPagedResponse()
    {
        await Sender.Send(new CreateBeltCommand() { BeltNumber = 2, ShortName = "гып", FullName = "Желтый пояс" });
        await Sender.Send(new CreateBeltCommand() { BeltNumber = 3, ShortName = "гып", FullName = "Черный пояс" });
        await Sender.Send(new CreateBeltCommand() { BeltNumber = 4, ShortName = "гып", FullName = "Красный пояс" });

        var getBeltListBySearch = new GetBeltListQuery() { Search = "пояс" };
        var getBeltListByOrder = new GetBeltListQuery() {OrderColumn = "beltNumber", OrderDir = "desc", Search = ""};

        var searchResponse = await Sender.Send(getBeltListBySearch);
        var orderColumnResponse = await Sender.Send(getBeltListByOrder);

        var dataBySearch = await DbContext.Belts.Where(x => x.FullName.Contains(getBeltListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.Belts.OrderByDescending(x => x.BeltNumber).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(belt => Mapper.Map<BeltResponse>(belt)).ToArray();
        var expectedOrder = dataByOrder.Select(belt => Mapper.Map<BeltResponse>(belt)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Belt_Must_BeRemoved()
    {
        var createBeltCommand = CreateBeltCommand();
        
        await Sender.Send(createBeltCommand);

        var beltId =  await DbContext.Belts.Where(x =>
            x.BeltNumber == createBeltCommand.BeltNumber && x.ShortName == createBeltCommand.ShortName)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveBeltCommand(beltId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Belts.AnyAsync(x => x.Id == beltId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Belt_Must_BeUpdated()
    {
        var beltId = await DbContext.Belts.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateBeltCommand() { Id = beltId, BeltNumber = 8, ShortName = "гып", FullName = "Черный пояс" };
        
        await Sender.Send(updateCommand);

        var updatedBelt = await DbContext.Belts.FindAsync(beltId);
        
        
        Assert.Equal(updateCommand.Id, updatedBelt.Id);
        Assert.Equal(updateCommand.ShortName, updatedBelt.ShortName);
        Assert.Equal(updateCommand.FullName, updatedBelt.FullName);
        Assert.Equal(updateCommand.BeltNumber, updatedBelt.BeltNumber);
    }
}