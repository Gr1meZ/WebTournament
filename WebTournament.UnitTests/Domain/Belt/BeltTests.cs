using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.UpdateBelt;
using WebTournament.Domain.Objects.Belt;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Belt;

public class BeltTests
{
     private readonly Mock<IBeltRepository> _beltRepository = new();
     
    [Fact]
    public async Task Belt_Must_BeCreated()
    {
        var createBeltCommand = new CreateBeltCommand()
        {
            BeltNumber = 1,
            ShortName = "гып",
            FullName = "Зеленый пояс"
        };
        
        _beltRepository.Setup(method => method.IsExistsAsync(1, "гып"))
            .ReturnsAsync(false);

        var belt = await BeltBuilder.BuildAsync(Guid.NewGuid());

        Assert.Equal(belt.BeltNumber, createBeltCommand.BeltNumber);
        Assert.Equal(belt.ShortName, createBeltCommand.ShortName);
        Assert.Equal(belt.FullName, createBeltCommand.FullName);
    }
    
    [Fact]
    public async Task Belt_AlreadyExists()
    {
        var createBeltCommand = new CreateBeltCommand()
        {
            BeltNumber = 1,
            ShortName = "гып",
            FullName = "Зеленый пояс"
        };
        
        _beltRepository.Setup(method => method.IsExistsAsync(1, "гып"))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Belt.Belt.CreateAsync(
            Guid.NewGuid(), createBeltCommand.BeltNumber.Value,
            createBeltCommand.ShortName,
            createBeltCommand.FullName, _beltRepository.Object));
    }
    
    [Fact]
    public async Task Belt_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateBeltCommand = new UpdateBeltCommand()
        {
            Id = id,
            BeltNumber = 2,
            ShortName = "дан",
            FullName = "Черный пояс"
        };
        
        var belt = await BeltBuilder.BuildAsync(id);

        _beltRepository.Setup(method => method.GetByIdAsync(updateBeltCommand.Id))
            .ReturnsAsync(belt);

        belt.Change(updateBeltCommand.BeltNumber.Value, updateBeltCommand.ShortName, updateBeltCommand.FullName);
        
        Assert.Equal(belt.Id, updateBeltCommand.Id);
        Assert.Equal(belt.BeltNumber, updateBeltCommand.BeltNumber);
        Assert.Equal(belt.ShortName, updateBeltCommand.ShortName);
        Assert.Equal(belt.FullName, updateBeltCommand.FullName);
    }
}