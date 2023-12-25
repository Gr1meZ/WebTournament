using DataAccess.Common.Exceptions;
using Moq;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Tests;

public class BeltServiceTest
{
    private readonly Mock<IBeltService> _beltServiceMock = new();


    [Fact]
    public async Task AddBeltAsync_NullModel_ThrowsValidationException()
    {
        // Arrange
        _beltServiceMock.Setup(service => service.AddBeltAsync(null))
            .Throws(new ValidationException("ValidationException", "Belt model is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _beltServiceMock.Object.AddBeltAsync(null));
    }

    [Fact]
    public async Task AddBeltAsync_IfModelIsValid()
    {
        // Arrange
        var beltViewModel = new BeltViewModel()
        {
            BeltNumber = 2,
            ShortName = "гып",
            FullName = "Black belt"
        };
        
        _beltServiceMock.Setup(service => service.AddBeltAsync(It.IsAny<BeltViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _beltServiceMock.Object.AddBeltAsync(beltViewModel);

        // Assert
        _beltServiceMock.Verify(m => m.AddBeltAsync(beltViewModel), Times.Once);
    }
    
    [Fact]
    public async Task DeleteBeltAsync_DeleteIfFound()
    {
        // Arrange
        var beltId = Guid.NewGuid();
        
        _beltServiceMock.Setup(service => service.DeleteBeltAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _beltServiceMock.Object.DeleteBeltAsync(beltId);

        // Assert
        _beltServiceMock.Verify(m => m.DeleteBeltAsync(beltId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteBeltAsync_IfBeltNotFound()
    {
        // Arrange
        _beltServiceMock.Setup(service => service.DeleteBeltAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Belt not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _beltServiceMock.Object.DeleteBeltAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task EditBeltAsync_IfNullModel()
    {
        // Arrange
        _beltServiceMock.Setup(service => service.EditBeltAsync(null))
            .Throws(new ValidationException("ValidationException","Age group not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _beltServiceMock.Object.EditBeltAsync(null));
    }
    
    [Fact]
    public async Task EditBeltAsync_IfModelValid()
    {
        // Arrange
        var beltViewModel = new BeltViewModel()
        {
            Id = new Guid(),
            BeltNumber = 2,
            ShortName = "гып",
            FullName = "Черный пояс"
        };
        
        _beltServiceMock.Setup(service => service.EditBeltAsync(It.IsAny<BeltViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _beltServiceMock.Object.EditBeltAsync(beltViewModel);

        // Assert
        _beltServiceMock.Verify(m => m.EditBeltAsync(beltViewModel), Times.Once);
    }
    
    [Fact]
    public async Task GetBeltAsync_IfBeltNotFound()
    {
        // Arrange
        _beltServiceMock.Setup(service => service.GetBeltAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","Belt not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _beltServiceMock.Object.GetBeltAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task GetBeltAsync_IfBeltIsFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var beltViewModel = new BeltViewModel()
        {
            Id = id,
            BeltNumber = 2,
            ShortName = "гып",
            FullName = "Черный пояс"
        };
        
        _beltServiceMock.Setup(service => service.GetBeltAsync(It.Is<Guid>(x => x != Guid.Empty)))
            .ReturnsAsync(() => beltViewModel);
        
        var result = await _beltServiceMock.Object.GetBeltAsync(id);
        // Act & Assert
        Assert.Equal(result.Id, id);
    }
    
    [Fact]
    public async Task BeltListAsync_ReturnsPagedResponse()
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
        var belts = BeltsList();
        var expectedResponse = new PagedResponse<BeltViewModel[]>
        (
            BeltsList() ,
            4, 
            request.PageNumber,
            request.PageSize
        );

        _beltServiceMock.Setup(service => service.BeltListAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
         var response = await _beltServiceMock.Object.BeltListAsync(request);

        // Assert
        _beltServiceMock.Verify(m => m.BeltListAsync(request), Times.Once);
        Assert.Equal(belts.Length, response.Metadata.TotalItemCount);
    }
    
    [Fact]
    public async Task GetSelect2BeltAsync_ReturnsSelect2Response()
    {
        // Arrange
        var request = new Select2Request()
        {
            PageSize = 10,
            Search = "",
            Skip = 0
        };
        
        var data = BeltsList().Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.ShortName
            })
            .ToArray();

        var expectedResponse = new Select2Response()
        {
            Data = data,
            Total = data.Length
        };

        _beltServiceMock.Setup(service => service.GetSelect2BeltsAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
        var response = await _beltServiceMock.Object.GetSelect2BeltsAsync(request);

        // Assert
        _beltServiceMock.Verify(m => m.GetSelect2BeltsAsync(request), Times.Once);
        Assert.Equal(expectedResponse, response);

    }
    
    private BeltViewModel[] BeltsList()
    {
        return new[]
        {
            new BeltViewModel
            {
                Id = Guid.NewGuid(),
                BeltNumber = 1,
                ShortName = "Белый",
                FullName = "Белый пояс"
            },
            new BeltViewModel
            {
                Id = Guid.NewGuid(),
                BeltNumber = 2,
                ShortName = "Желтый",
                FullName = "Желтый пояс"
            },
            new BeltViewModel
            {
                Id = Guid.NewGuid(),
                BeltNumber = 3,
                ShortName = "гып",
                FullName = "Желтый пояс"
            },
            new BeltViewModel
            {
                Id = Guid.NewGuid(),
                BeltNumber = 4,
                ShortName = "дан",
                FullName = "Желтый пояс"
            },
        };
    }
}
