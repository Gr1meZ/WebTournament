using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.UpdateClub;
using WebTournament.Domain.Objects.Club;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Club;

public class ClubTests
{
    private readonly Mock<IClubRepository> _clubRepository = new();
     
    [Fact]
    public async Task Club_Must_BeCreated()
    {
        var createClubCommand = new CreateClubCommand() { Name = "Юнош" };
        
        _clubRepository.Setup(method => method.IsExistsAsync("Юнош"))
            .ReturnsAsync(false);

        var сlub = await ClubBuilder.BuildAsync(Guid.NewGuid());

        Assert.Equal(сlub.Name, createClubCommand.Name);
      
    }
    
    [Fact]
    public async Task Club_AlreadyExists()
    {
        var createClubCommand = new CreateClubCommand() { Name = "Юнош" };
        
        _clubRepository.Setup(method => method.IsExistsAsync("Юнош"))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Club.Club.CreateAsync(
            Guid.NewGuid(), createClubCommand.Name, _clubRepository.Object));
    }
    
    [Fact]
    public async Task Club_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateClubCommand = new UpdateClubCommand()
        {
            Id = id,
            Name = "Свобода"
        };
        
        var club = await ClubBuilder.BuildAsync(id);

        _clubRepository.Setup(method => method.GetByIdAsync(updateClubCommand.Id))
            .ReturnsAsync(club);

        club.Change(updateClubCommand.Name);
        
        Assert.Equal(club.Id, updateClubCommand.Id);
        Assert.Equal(club.Name, updateClubCommand.Name);
    }
}