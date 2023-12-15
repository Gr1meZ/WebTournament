using DataAccess.Common.Exceptions;
using Moq;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Tests;

public class ClubServiceTest
{
    private readonly Mock<IClubService> _clubServiceMock = new();


    [Fact]
    public async Task AddClubAsync_NullModel_ThrowsValidationException()
    {
        // Arrange
        _clubServiceMock.Setup(service => service.AddClubAsync(null))
            .Throws(new ValidationException("ValidationException", "Club model is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _clubServiceMock.Object.AddClubAsync(null));
    }

    [Fact]
    public async Task AddClubAsync_IfModelIsValid()
    {
        // Arrange
        var ClubViewModel = new ClubViewModel()
        {
            Name = "Юнош"
        };
        
        _clubServiceMock.Setup(service => service.AddClubAsync(It.IsAny<ClubViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _clubServiceMock.Object.AddClubAsync(ClubViewModel);

        // Assert
        _clubServiceMock.Verify(m => m.AddClubAsync(ClubViewModel), Times.Once);
    }
    
    [Fact]
    public async Task DeleteClubAsync_DeleteIfFound()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        
        _clubServiceMock.Setup(service => service.DeleteClubAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _clubServiceMock.Object.DeleteClubAsync(clubId);

        // Assert
        _clubServiceMock.Verify(m => m.DeleteClubAsync(clubId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteClubAsync_IfClubNotFound()
    {
        // Arrange
        _clubServiceMock.Setup(service => service.DeleteClubAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Club not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _clubServiceMock.Object.DeleteClubAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task EditClubAsync_IfNullModel()
    {
        // Arrange
        _clubServiceMock.Setup(service => service.EditClubAsync(null))
            .Throws(new ValidationException("ValidationException","Club not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _clubServiceMock.Object.EditClubAsync(null));
    }
    
    [Fact]
    public async Task EditClubAsync_IfModelValid()
    {
        // Arrange
        var clubViewModel = new ClubViewModel()
        {
            Id = new Guid(),
            Name = "Юнош"
        };
        
        _clubServiceMock.Setup(service => service.EditClubAsync(It.IsAny<ClubViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _clubServiceMock.Object.EditClubAsync(clubViewModel);

        // Assert
        _clubServiceMock.Verify(m => m.EditClubAsync(clubViewModel), Times.Once);
    }
    
    [Fact]
    public async Task GetClubAsync_IfClubNotFound()
    {
        // Arrange
        _clubServiceMock.Setup(service => service.GetClubAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Club not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _clubServiceMock.Object.GetClubAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task GetClubAsync_IfClubIsFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var clubViewModel = new ClubViewModel()
        {
            Id = id,
            Name = "Юнош"
        };
        
        _clubServiceMock.Setup(service => service.GetClubAsync(It.Is<Guid>(x => x != Guid.Empty)))
            .ReturnsAsync(() => clubViewModel);
        
        var result = await _clubServiceMock.Object.GetClubAsync(id);
        // Act & Assert
        Assert.Equal(result.Id, id);
    }
    
    [Fact]
    public async Task ClubListAsync_ReturnsPagedResponse()
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
        var Clubs = СlubList();
        var expectedResponse = new PagedResponse<ClubViewModel[]>
        (
            СlubList() ,
            4, 
            request.PageNumber,
            request.PageSize
        );

        _clubServiceMock.Setup(service => service.ClubListAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
         var response = await _clubServiceMock.Object.ClubListAsync(request);

        // Assert
        _clubServiceMock.Verify(m => m.ClubListAsync(request), Times.Once);
        Assert.Equal(Clubs.Length, response.Metadata.TotalItemCount);
    }
    
    [Fact]
    public async Task GetSelect2ClubAsync_ReturnsSelect2Response()
    {
        // Arrange
        var request = new Select2Request()
        {
            PageSize = 10,
            Search = "",
            Skip = 0
        };
        
        var data = СlubList().Select(x => new Select2Data()
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

        _clubServiceMock.Setup(service => service.GetSelect2ClubsAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
        var response = await _clubServiceMock.Object.GetSelect2ClubsAsync(request);

        // Assert
        _clubServiceMock.Verify(m => m.GetSelect2ClubsAsync(request), Times.Once);
        Assert.Equal(expectedResponse, response);

    }
    
    private ClubViewModel[] СlubList()
    {
        return new[]
        {
            new ClubViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Юнош"
            },
            new ClubViewModel
            {
                Id = Guid.NewGuid(),
                Name = "ФайтКлуб"
            },
            new ClubViewModel
            {
                Id = Guid.NewGuid(),
                Name = "СпортГородок"
            },
            new ClubViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Свобода"
            },
        };
    }
}
