using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Club;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.GetClub;
using WebTournament.Application.Club.GetClubList;
using WebTournament.Application.Club.RemoveClub;
using WebTournament.Application.Club.UpdateClub;

namespace WebTournament.IntegrationTests.Club;

public class ClubTests : BaseIntegrationTest
{
    public ClubTests(WebApplicationFactory factory) : base(factory)
    {
    }
     [Fact]
    public async Task Club_Must_BeCreated()
    {
        var createClubCommand = new CreateClubCommand() { Name = "Юнош" };
        
        await Sender.Send(createClubCommand);
        
        var club = await DbContext.Clubs.FirstOrDefaultAsync(x => x.Name == createClubCommand.Name);
        
        Assert.NotNull(club);
    }
    
    [Fact]
    public async Task Club_AlreadyExists_ThrowsException()
    {
        var createClubCommand = new CreateClubCommand() { Name = "СпортКлуб" };
        
        await Sender.Send(createClubCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createClubCommand));
    }
    
    [Fact]
    public async Task GetClub_IsValid()
    {
        var clubId = await DbContext.Clubs.Select(x => x.Id).FirstOrDefaultAsync();
        var getClubQuery = new GetClubQuery(clubId);
        
        var clubResponse = await Sender.Send(getClubQuery);
        
        Assert.NotNull(clubResponse);
    }
    
    [Fact]
    public async Task GetClub_NotFound_ThrowsException()
    {
        var getClubQuery = new GetClubQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getClubQuery));
    }
    
    [Fact]
    public async Task GetClubList_Must_ReturnValidPagedResponse()
    {
        await Sender.Send(new CreateClubCommand() { Name = "ЮношКлуб" });
        await Sender.Send(new CreateClubCommand() { Name = "СвободаКлуб" });
        await Sender.Send(new CreateClubCommand() { Name = "ТестКлуб" });

        var getClubListBySearch = new GetClubListQuery() { Search = "Клуб" };
        var getClubListByOrder = new GetClubListQuery() {OrderColumn = "name", OrderDir = "desc", Search = ""};

        var searchResponse = await Sender.Send(getClubListBySearch);
        var orderColumnResponse = await Sender.Send(getClubListByOrder);

        var dataBySearch = await DbContext.Clubs.Where(x => x.Name.Contains(getClubListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.Clubs.OrderByDescending(x => x.Name).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(club => Mapper.Map<ClubResponse>(club)).ToArray();
        var expectedOrder = dataByOrder.Select(club => Mapper.Map<ClubResponse>(club)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Club_Must_BeRemoved()
    {
        var createClubCommand = new CreateClubCommand() { Name = "ТестКлуб" };
      
        
        await Sender.Send(createClubCommand);

        var clubId =  await DbContext.Clubs
            .Where(x => x.Name == createClubCommand.Name)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveClubCommand(clubId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Clubs.AnyAsync(x => x.Id == clubId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Club_Must_BeUpdated()
    {
        var clubId = await DbContext.Clubs.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateClubCommand() { Id = clubId,Name = "СпортМастер"};
        
        await Sender.Send(updateCommand);

        var updatedClub = await DbContext.Clubs.FindAsync(clubId);
        
        
        Assert.Equal(updateCommand.Id, updatedClub.Id);
        Assert.Equal(updateCommand.Name, updatedClub.Name);
    }
}