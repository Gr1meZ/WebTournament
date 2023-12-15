using DataAccess.Common.Exceptions;
using Moq;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Tests;

public class TrainerServiceTest
{
    private readonly Mock<ITrainerService> _trainerServiceMock = new();


    [Fact]
    public async Task AddTrainerAsync_NullModel_ThrowsValidationException()
    {
        // Arrange
        _trainerServiceMock.Setup(service => service.AddTrainerAsync(null))
            .Throws(new ValidationException("ValidationException", "Trainer model is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _trainerServiceMock.Object.AddTrainerAsync(null));
    }

    [Fact]
    public async Task AddTrainerAsync_IfModelIsValid()
    {
        // Arrange
        var trainerViewModel = new TrainerViewModel()
        {
            Name = "Юнош",
            Patronymic = "Игоревич",
            Phone = "+375257635337",
            Surname = "Иванов",
            ClubId = Guid.NewGuid(),
            ClubName = "Юнош"
        };
        
        _trainerServiceMock.Setup(service => service.AddTrainerAsync(It.IsAny<TrainerViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _trainerServiceMock.Object.AddTrainerAsync(trainerViewModel);

        // Assert
        _trainerServiceMock.Verify(m => m.AddTrainerAsync(trainerViewModel), Times.Once);
    }
    
    [Fact]
    public async Task DeleteTrainerAsync_DeleteIfFound()
    {
        // Arrange
        var trainerId = Guid.NewGuid();
        
        _trainerServiceMock.Setup(service => service.DeleteTrainerAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _trainerServiceMock.Object.DeleteTrainerAsync(trainerId);

        // Assert
        _trainerServiceMock.Verify(m => m.DeleteTrainerAsync(trainerId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteTrainerAsync_IfTrainerNotFound()
    {
        // Arrange
        _trainerServiceMock.Setup(service => service.DeleteTrainerAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Trainer not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _trainerServiceMock.Object.DeleteTrainerAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task EditTrainerAsync_IfNullModel()
    {
        // Arrange
        _trainerServiceMock.Setup(service => service.EditTrainerAsync(null))
            .Throws(new ValidationException("ValidationException","Trainer not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _trainerServiceMock.Object.EditTrainerAsync(null));
    }
    
    [Fact]
    public async Task EditTrainerAsync_IfModelValid()
    {
        // Arrange
        var trainerViewModel = new TrainerViewModel()
        {
            Name = "Юнош",
            Patronymic = "Игоревич",
            Phone = "+375257635337",
            Surname = "Иванов",
            ClubId = Guid.NewGuid(),
            ClubName = "Юнош"
        };
        
        _trainerServiceMock.Setup(service => service.EditTrainerAsync(It.IsAny<TrainerViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _trainerServiceMock.Object.EditTrainerAsync(trainerViewModel);

        // Assert
        _trainerServiceMock.Verify(m => m.EditTrainerAsync(trainerViewModel), Times.Once);
    }
    
    [Fact]
    public async Task GetTrainerAsync_IfTrainerNotFound()
    {
        // Arrange
        _trainerServiceMock.Setup(service => service.GetTrainerAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Trainer not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _trainerServiceMock.Object.GetTrainerAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task GetTrainerAsync_IfTrainerIsFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var trainerViewModel = new TrainerViewModel()
        {
            Name = "Юнош",
            Patronymic = "Игоревич",
            Phone = "+375257635337",
            Surname = "Иванов",
            ClubId = Guid.NewGuid(),
            ClubName = "Юнош",
            Id = id
        };
        
        _trainerServiceMock.Setup(service => service.GetTrainerAsync(It.Is<Guid>(x => x != Guid.Empty)))
            .ReturnsAsync(() => trainerViewModel);
        
        var result = await _trainerServiceMock.Object.GetTrainerAsync(id);
        // Act & Assert
        Assert.Equal(result.Id, id);
    }
    
    [Fact]
    public async Task TrainerListAsync_ReturnsPagedResponse()
    {
        // Arrange
        var request = new PagedRequest
        {
            PageNumber = 1,
            PageSize = 10,
            Search = "",
            OrderColumn = "test",
            OrderDir = "asc"
        };
        var Trainers = TrainerList();
        var expectedResponse = new PagedResponse<TrainerViewModel[]>
        (
            TrainerList() ,
            4, 
            request.PageNumber,
            request.PageSize
        );

        _trainerServiceMock.Setup(service => service.TrainersListAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
         var response = await _trainerServiceMock.Object.TrainersListAsync(request);

        // Assert
        _trainerServiceMock.Verify(m => m.TrainersListAsync(request), Times.Once);
        Assert.Equal(Trainers.Length, response.Metadata.TotalItemCount);
    }
    
    [Fact]
    public async Task GetSelect2TrainerAsync_ReturnsSelect2Response()
    {
        // Arrange
        var request = new Select2Request()
        {
            PageSize = 10,
            Search = "",
            Skip = 0
        };
        
        var data = TrainerList().Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToArray();

        var expectedResponse = new Select2Response()
        {
            Data = data,
            Total = data.Length
        };

        _trainerServiceMock.Setup(service => service.GetAutoCompleteTrainersAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
        var response = await _trainerServiceMock.Object.GetAutoCompleteTrainersAsync(request);

        // Assert
        _trainerServiceMock.Verify(m => m.GetAutoCompleteTrainersAsync(request), Times.Once);
        Assert.Equal(expectedResponse, response);

    }
    
    private TrainerViewModel[] TrainerList()
    {
        return new[]
        {
            new TrainerViewModel
            {
                Id = Guid.NewGuid(),
                ClubId = Guid.NewGuid(),
                ClubName = "Клуб 1",
                Name = "Имя 1",
                Surname = "Фамилия 1",
                Patronymic = "Отчество 1",
                Phone = "+375 29 1234567"
            },
            new TrainerViewModel
            {
                Id = Guid.NewGuid(),
                ClubId = Guid.NewGuid(),
                ClubName = "Клуб 2",
                Name = "Имя 2",
                Surname = "Фамилия 2",
                Patronymic = "Отчество 2",
                Phone = "+375 29 7654321"
            },
            new TrainerViewModel
            {
                Id = Guid.NewGuid(),
                ClubId = Guid.NewGuid(),
                ClubName = "Клуб 3",
                Name = "Имя 3",
                Surname = "Фамилия 3",
                Patronymic = "Отчество 3",
                Phone = "+375 29 1234567"
            },
            new TrainerViewModel
            {
                Id = Guid.NewGuid(),
                ClubId = Guid.NewGuid(),
                ClubName = "Клуб 4",
                Name = "Имя 4",
                Surname = "Фамилия 4",
                Patronymic = "Отчество 4",
                Phone = "+375 29 7654321"
            }
        };
    }
}
