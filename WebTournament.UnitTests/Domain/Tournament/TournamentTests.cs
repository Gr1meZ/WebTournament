using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Tournament.UpdateTournament;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Tournament;

public class TournamentTests
{
    private readonly Mock<ITournamentRepository> _tournamentRepository = new();
     
    [Fact]
    public async Task Tournament_Must_BeCreated()
    {
        var createTournamentCommand = new CreateTournamentCommand()
        {
            Name = "Minsk Cup 2023",
            Address = "Улица Победы 14",
            StartDate = DateTime.UtcNow
        };
        
        _tournamentRepository.Setup(method => method.IsExistsAsync(createTournamentCommand.Name, createTournamentCommand.Address))
            .ReturnsAsync(false);

        var tournament = await TournamentBuilder.BuildAsync(Guid.NewGuid(), createTournamentCommand.StartDate);
        
        Assert.Equal(tournament.Name, createTournamentCommand.Name);
        Assert.Equal(tournament.Address, createTournamentCommand.Address);
        Assert.Equal(tournament.StartDate, createTournamentCommand.StartDate);

    }
    
    [Fact]
    public async Task Tournament_AlreadyExists()
    {
        var createTournamentCommand = new CreateTournamentCommand()
        {
            Name = "Minsk Cup 2023",
            Address = "Улица Победы 14",
            StartDate = DateTime.UtcNow
        };
        
        _tournamentRepository.Setup(method => method.IsExistsAsync(createTournamentCommand.Name, createTournamentCommand.Address))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Tournament.Tournament.CreateAsync(
            Guid.NewGuid(), createTournamentCommand.Name, createTournamentCommand.StartDate, 
            createTournamentCommand.Address, _tournamentRepository.Object));
    }
    
    [Fact]
    public async Task Tournament_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateTournamentCommand = new UpdateTournamentCommand()
        {
            Id = id,
            Name = "Minsk Cup 2023",
            Address = "Улица Победы 14",
            StartDate = DateTime.UtcNow
        };
        
        var tournament = await TournamentBuilder.BuildAsync(id);
        
        _tournamentRepository.Setup(method => method.GetByIdAsync(updateTournamentCommand.Id))
            .ReturnsAsync(tournament);

        tournament.Change(updateTournamentCommand.Name, updateTournamentCommand.StartDate, updateTournamentCommand.Address);
        
        Assert.Equal(tournament.Id, updateTournamentCommand.Id);
        Assert.Equal(tournament.Name, updateTournamentCommand.Name);
        Assert.Equal(tournament.Address, updateTournamentCommand.Address);
        Assert.Equal(tournament.StartDate, updateTournamentCommand.StartDate);

    }
}