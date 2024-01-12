using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Bracket;
using WebTournament.Application.Bracket.DistributePlayers;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Bracket.SaveBracketState;
using WebTournament.Application.Tournament;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Tournament.GetTournament;
using WebTournament.Application.Tournament.GetTournamentList;
using WebTournament.Application.Tournament.GetTournamentResults;
using WebTournament.Application.Tournament.RemoveTournament;
using WebTournament.Application.Tournament.UpdateTournament;

namespace WebTournament.IntegrationTests.Tournament;

public class TournamentTests : BaseIntegrationTest
{
    public TournamentTests(WebApplicationFactory factory) : base(factory)
    {
    }
    
      [Fact]
    public async Task Tournament_Must_BeCreated()
    {
        var createTournamentCommand = CreateTournamentCommand();
        
        await Sender.Send(createTournamentCommand);
        
        var tournament = await DbContext.Tournaments
            .FirstOrDefaultAsync(x => x.Name == createTournamentCommand.Name 
                                      && x.Address == createTournamentCommand.Address
                                      && x.StartDate == createTournamentCommand.StartDate);
        
        Assert.NotNull(tournament);
    }
    
    [Fact]
    public async Task Tournament_AlreadyExists_ThrowsException()
    {
        var createTournamentCommand = CreateTournamentCommand();
        
        await Sender.Send(createTournamentCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createTournamentCommand));
    }
    
    [Fact]
    public async Task GetTournament_IsValid()
    {
        var tournamentId = await DbContext.Tournaments.Select(x => x.Id).FirstOrDefaultAsync();
        var getTournamentQuery = new GetTournamentQuery(tournamentId);
        
        var tournamentResponse = await Sender.Send(getTournamentQuery);
        
        Assert.NotNull(tournamentResponse);
    }
    
    [Fact]
    public async Task GetTournament_NotFound_ThrowsException()
    {
        var getTournamentQuery = new GetTournamentQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getTournamentQuery));
    }
    
    [Fact]
    public async Task GetTournamentList_Must_ReturnValidPagedResponse()
    {
        await Sender.Send(new CreateTournamentCommand() 
        { 
            Name = "Minsk Cup 2023", 
            Address = Guid.NewGuid().ToString(), 
            StartDate = DateTime.UtcNow.AddDays(new Random().Next(1, 50)) 
        } );
        
        await Sender.Send(new CreateTournamentCommand() 
        { 
            Name = "Minsk Cup 2022", 
            Address = Guid.NewGuid().ToString(), 
            StartDate = DateTime.UtcNow.AddDays(new Random().Next(1, 50)) 
        } );

        var getTournamentListBySearch = new GetTournamentListQuery() { Search = "Cup" };
        var getTournamentListByOrder = new GetTournamentListQuery() {OrderColumn = "name", OrderDir = "desc", Search = ""};

        var searchResponse = await Sender.Send(getTournamentListBySearch);
        var orderColumnResponse = await Sender.Send(getTournamentListByOrder);

        var dataBySearch = await DbContext.Tournaments.Where(x => x.Name.Contains(getTournamentListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.Tournaments.OrderByDescending(x => x.Name).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(tournament => Mapper.Map<TournamentResponse>(tournament)).ToArray();
        var expectedOrder = dataByOrder.Select(tournament => Mapper.Map<TournamentResponse>(tournament)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Tournament_Must_BeRemoved()
    {
        var createTournamentCommand = CreateTournamentCommand();
      
        
        await Sender.Send(createTournamentCommand);

        var tournamentId =  await DbContext.Tournaments
            .Where(x => x.Name == createTournamentCommand.Name)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveTournamentCommand(tournamentId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Tournaments.AnyAsync(x => x.Id == tournamentId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Tournament_Must_BeUpdated()
    {
        var tournamentId = await DbContext.Tournaments.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateTournamentCommand()
        {
            Id = tournamentId,
            Name = Guid.NewGuid().ToString(),
            Address = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow.AddDays(new Random().Next(0, 50))
        };
        
        await Sender.Send(updateCommand);

        var updatedTournament = await DbContext.Tournaments.FindAsync(tournamentId);
        
        
        Assert.Equal(updateCommand.Id, updatedTournament.Id);
        Assert.Equal(updateCommand.Name, updatedTournament.Name);
        Assert.Equal(updateCommand.Address, updatedTournament.Address);
        Assert.Equal(updateCommand.StartDate, updatedTournament.StartDate);
    }
    
    [Fact]
    public async Task GetTournamentResults_Must_BeValid()
    {
        var bracketCmd = await CreateBracketCommand();
        await Sender.Send(bracketCmd);
        await Sender.Send(new DistributePlayersCommand(bracketCmd.TournamentId));
        
        var winners = await DbContext.Fighters
            .Where(x => x.TournamentId == bracketCmd.TournamentId)
            .Take(3)
            .Select(x => x.Id)
            .ToListAsync();
        
        var bracketId = await DbContext.Brackets
            .Where(x => x.TournamentId == bracketCmd.TournamentId
                        && x.Division == bracketCmd.Division)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        var bracketState = new BracketStateRequest()
        {
            State = "Some state",
            Winners = winners,
            Id = bracketId
        };       
        
        await Sender.Send(new SaveBracketStateCommand(bracketId, bracketState));
        var winnersResponse = await Sender
            .Send(new GetTournamentResultsQuery(bracketCmd.TournamentId));
        
        var type = typeof(BracketWinnerResponse);
        var properties = type.GetProperties();
        foreach (var value in from winner in winnersResponse from property in properties select property.GetValue(winner))
        {
            Assert.NotNull(value);
        }
    }
}