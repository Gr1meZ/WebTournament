using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Trainer;

public class TrainerTests
{
     private readonly Mock<ITrainerRepository> _trainerRepository = new();
     
    [Fact]
    public async Task Trainer_Must_BeCreated()
    {
        var createTrainerCommand = new CreateTrainerCommand()
        {
            Name = "Иван",
            Patronymic = "Иванович",
            Surname = "Иванов",
            Phone = "+375253333333",
            ClubId = Guid.NewGuid()
        };
        
        _trainerRepository.Setup(method => method.IsExistsAsync(createTrainerCommand.Name, createTrainerCommand.Surname, 
                createTrainerCommand.Patronymic, createTrainerCommand.Phone, createTrainerCommand.ClubId.Value))
            .ReturnsAsync(false);

        var trainer = await TrainerBuilder.BuildAsync(Guid.NewGuid(), createTrainerCommand.ClubId.Value);
        
        Assert.Equal(trainer.Name, createTrainerCommand.Name);
        Assert.Equal(trainer.Surname, createTrainerCommand.Surname);
        Assert.Equal(trainer.Patronymic, createTrainerCommand.Patronymic);
        Assert.Equal(trainer.Phone, createTrainerCommand.Phone);
        Assert.Equal(trainer.ClubId, createTrainerCommand.ClubId);

    }
    
    [Fact]
    public async Task Trainer_AlreadyExists()
    {
        var createTrainerCommand = new CreateTrainerCommand()
        {
            Name = "Иван",
            Patronymic = "Иванович",
            Surname = "Иванов",
            Phone = "+375253333333",
            ClubId = Guid.NewGuid()
        };
        
        _trainerRepository.Setup(method => method.IsExistsAsync(createTrainerCommand.Name, createTrainerCommand.Surname, 
                createTrainerCommand.Patronymic, createTrainerCommand.Phone, createTrainerCommand.ClubId.Value))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Trainer.Trainer.CreateAsync(
            Guid.NewGuid(), createTrainerCommand.Name, createTrainerCommand.Surname, 
            createTrainerCommand.Patronymic, createTrainerCommand.Phone, createTrainerCommand.ClubId.Value, _trainerRepository.Object));
    }
    
    [Fact]
    public async Task Trainer_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateTrainerCommand = new UpdateTrainerCommand()
        {
            Id = id,
            Name = "Артем",
            Patronymic = "Игоревич",
            Surname = "Морозов",
            Phone = "+37525333444",
            ClubId = Guid.NewGuid()
        };
        
        var trainer = await TrainerBuilder.BuildAsync(id);
        
        _trainerRepository.Setup(method => method.GetByIdAsync(updateTrainerCommand.Id))
            .ReturnsAsync(trainer);

        trainer.Change(updateTrainerCommand.Name, updateTrainerCommand.Surname, updateTrainerCommand.Patronymic, updateTrainerCommand.Phone, 
            updateTrainerCommand.ClubId.Value);
        
        Assert.Equal(trainer.Id, updateTrainerCommand.Id);
        Assert.Equal(trainer.Name, updateTrainerCommand.Name);
        Assert.Equal(trainer.Surname, updateTrainerCommand.Surname);
        Assert.Equal(trainer.Patronymic, updateTrainerCommand.Patronymic);
        Assert.Equal(trainer.Phone, updateTrainerCommand.Phone);
        Assert.Equal(trainer.ClubId, updateTrainerCommand.ClubId);
    }
}