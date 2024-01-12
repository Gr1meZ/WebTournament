using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Bracket;
using WebTournament.Application.Bracket.DistributePlayers;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Bracket.GetBracket;
using WebTournament.Application.Bracket.GetBracketList;
using WebTournament.Application.Bracket.RemoveAllBrackets;
using WebTournament.Application.Bracket.RemoveBracket;
using WebTournament.Application.Bracket.SaveBracketState;
using WebTournament.Application.Fighter.CreateFighter;
using ValidationException = CustomExceptionsLibrary.ValidationException;

namespace WebTournament.IntegrationTests.Bracket;

public class BracketTests : BaseIntegrationTest
{
    public BracketTests(WebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Bracket_Must_BeGenerated()
    {
        var generateBracketCmd = await CreateBracketCommand();

        await Sender.Send(generateBracketCmd);

        var brackets = await DbContext.Brackets.Where(x =>
                x.TournamentId == generateBracketCmd.TournamentId && x.WeightCategorieId == generateBracketCmd.Id)
            .ToListAsync();

        Assert.Single(brackets);
    }

    [Fact]
    public async Task Players_Must_BeDistributed()
    {
        var generateBracketCmd = await CreateBracketCommand();
        await Sender.Send(generateBracketCmd);

        var distributePlayersCmd = new DistributePlayersCommand(generateBracketCmd.TournamentId);
        await Sender.Send(distributePlayersCmd);

        var isFightersHaveBracket = DbContext.Fighters
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId)
            .All(x => x.BracketId != null);

        var bracketStateUpdated = DbContext.Brackets
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId)
            .All(x => x.State != string.Empty);

        var bracketWinnerCreated = await DbContext.BracketWinners
            .Where(x => x.Bracket.TournamentId == generateBracketCmd.TournamentId)
            .AnyAsync();

        Assert.True(isFightersHaveBracket);
        Assert.True(bracketStateUpdated);
        Assert.True(bracketWinnerCreated);
    }

    [Fact]
    public async Task GetBracket_Must_BeValid()
    {
        var bracket = await DbContext.Brackets.FirstOrDefaultAsync();
        var response = await Sender.Send(new GetBracketQuery(bracket.Id));

        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetBracket_NotFound_MustThrowException()
    {
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(new GetBracketQuery(Guid.NewGuid())));
    }

    [Fact]
    public async Task GetBracketList_Must_ReturnValidPagedResponse()
    {
        var generateBracketCmd = await CreateBracketCommand();
        await Sender.Send(generateBracketCmd);

        var response = await Sender.Send(new GetBracketListQuery(generateBracketCmd.TournamentId) {Search = string.Empty, OrderDir = "asc"});
        var expectedCount = await DbContext.Brackets
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId)
            .CountAsync();

        Assert.Equal(expectedCount, response.Metadata.TotalItemCount);
    }
    
    [Fact]
    public async Task AllBracket_Must_BeRemoved()
    {
        var generateBracketCmd = await CreateBracketCommand();
        await Sender.Send(new RemoveAllBracketsCommand(generateBracketCmd.TournamentId));

        var isBracketsDeleted =
            !await DbContext.Brackets.AnyAsync(x => x.TournamentId == generateBracketCmd.TournamentId);

        Assert.True(isBracketsDeleted);
    }
    
    [Fact]
    public async Task Bracket_Must_BeRemoved()
    {
        var generateBracketCmd = await CreateBracketCommand();
        await Sender.Send(generateBracketCmd);
        var bracketId = await DbContext.Brackets
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        await Sender.Send(new RemoveBracketCommand(bracketId));

        var isBracketDeleted = !await DbContext.Brackets.AnyAsync(x => x.Id == bracketId);

        Assert.True(isBracketDeleted);
    }
    
    [Fact]
    public async Task BracketState_Must_BeUpdated()
    {
        var generateBracketCmd = await CreateBracketCommand();
        await Sender.Send(generateBracketCmd);
        
        await Sender.Send(new DistributePlayersCommand(generateBracketCmd.TournamentId));
        
        var bracketId = await DbContext.Brackets
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId
            && x.Division == generateBracketCmd.Division)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        var bracketWinnerId = await DbContext.BracketWinners
            .Include(x => x.Bracket)
            .Where(x => x.Bracket.TournamentId == generateBracketCmd.TournamentId && x.Id == bracketId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        var winners = await DbContext.Fighters
            .Where(x => x.TournamentId == generateBracketCmd.TournamentId)
            .Take(3)
            .Select(x => x.Id)
            .ToListAsync();
        
        var bracketState = new BracketStateRequest()
        {
            State = "Some state",
            Winners = winners,
            Id = bracketWinnerId
        };       
        
        await Sender.Send(new SaveBracketStateCommand(bracketId, bracketState));
        var updatedBracket = await DbContext.Brackets.FindAsync(bracketId);
        var bracketWinners = await DbContext.BracketWinners.FindAsync(bracketId);
        
        Assert.Equal(bracketState.State, updatedBracket.State);
        Assert.NotNull(bracketWinners.FirstPlaceId);
        Assert.NotNull(bracketWinners.SecondPlaceId);
        Assert.NotNull(bracketWinners.ThirdPlaceId);

    }
}