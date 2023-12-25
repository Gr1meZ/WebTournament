using DataAccess.Common.Enums;
using DataAccess.Common.Exceptions;
using DataAccess.Common.Extensions;
using Moq;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Tests;

public class WeightCategorieServiceTest
{
    private readonly Mock<IWeightCategorieService> _weightCategorieServiceMock  = new();


    [Fact]
    public async Task AddWeightCategorieAsync_NullModel_ThrowsValidationException()
    {
        // Arrange
        _weightCategorieServiceMock.Setup(service => service.AddWeightCategorieAsync(null))
            .Throws(new ValidationException("ValidationException", "Weight Categorie model is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _weightCategorieServiceMock.Object.AddWeightCategorieAsync(null));
    }

    [Fact]
    public async Task AddWeightCategorieAsync_IfModelIsValid()
    {
        // Arrange
        var weightCategorieViewModel = new WeightCategorieViewModel()
        {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 20,
            WeightName = "Биг матсоги",
            AgeGroupName = "Возрастная группа 5-6 лет"
        };
        
        _weightCategorieServiceMock.Setup(service => service.AddWeightCategorieAsync(It.IsAny<WeightCategorieViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _weightCategorieServiceMock.Object.AddWeightCategorieAsync(weightCategorieViewModel);

        // Assert
        _weightCategorieServiceMock.Verify(m => m.AddWeightCategorieAsync(weightCategorieViewModel), Times.Once);
    }
    
    [Fact]
    public async Task DeleteWeightCategorieAsync_DeleteIfFound()
    {
        // Arrange
        var weightCategorieId = Guid.NewGuid();
        
        _weightCategorieServiceMock.Setup(service => service.DeleteWeightCategorieAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _weightCategorieServiceMock.Object.DeleteWeightCategorieAsync(weightCategorieId);

        // Assert
        _weightCategorieServiceMock.Verify(m => m.DeleteWeightCategorieAsync(weightCategorieId), Times.Once);
    }
    
    [Fact]
    public async Task DeleteWeightCategorieAsync_IfWeightCategorieNotFound()
    {
        // Arrange
        _weightCategorieServiceMock.Setup(service => service.DeleteWeightCategorieAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","WeightCategorie not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _weightCategorieServiceMock.Object.DeleteWeightCategorieAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task EditWeightCategorieAsync_IfNullModel()
    {
        // Arrange
        _weightCategorieServiceMock.Setup(service => service.EditWeightCategorieAsync(null))
            .Throws(new ValidationException("ValidationException","WeightCategorie not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _weightCategorieServiceMock.Object.EditWeightCategorieAsync(null));
    }
    
    [Fact]
    public async Task EditWeightCategorieAsync_IfModelValid()
    {
        // Arrange
        var weightCategorieViewModel = new WeightCategorieViewModel()
        {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 20,
            WeightName = "Биг матсоги",
            AgeGroupName = "Возрастная группа 5-6 лет"
        };
        
        _weightCategorieServiceMock.Setup(service => service.EditWeightCategorieAsync(It.IsAny<WeightCategorieViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _weightCategorieServiceMock.Object.EditWeightCategorieAsync(weightCategorieViewModel);

        // Assert
        _weightCategorieServiceMock.Verify(m => m.EditWeightCategorieAsync(weightCategorieViewModel), Times.Once);
    }
    
    [Fact]
    public async Task GetWeightCategorieAsync_IfWeightCategorieNotFound()
    {
        // Arrange
        _weightCategorieServiceMock.Setup(service => service.GetWeightCategorieAsync(Guid.Empty))
            .Throws(new ValidationException("ValidationException","WeightCategorie not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _weightCategorieServiceMock.Object.GetWeightCategorieAsync(Guid.Empty));
    }
    
    [Fact]
    public async Task GetWeightCategorieAsync_IfWeightCategorieIsFounded()
    {
        // Arrange
        var id = Guid.NewGuid();
        var weightCategorieViewModel = new WeightCategorieViewModel()
        {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 20,
            WeightName = "Биг матсоги",
            AgeGroupName = "Возрастная группа 5-6 лет",
            Id = id
        };
        
        _weightCategorieServiceMock.Setup(service => service.GetWeightCategorieAsync(It.Is<Guid>(x => x != Guid.Empty)))
            .ReturnsAsync(() => weightCategorieViewModel);
        
        var result = await _weightCategorieServiceMock.Object.GetWeightCategorieAsync(id);
        // Act & Assert
        Assert.Equal(result.Id, id);
    }
    
    [Fact]
    public async Task WeightCategorieListAsync_ReturnsPagedResponse()
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
        var weightCategories = WeightCategorieList();
        var expectedResponse = new PagedResponse<WeightCategorieViewModel[]>
        (
            WeightCategorieList() ,
            5, 
            request.PageNumber,
            request.PageSize
        );

        _weightCategorieServiceMock.Setup(service => service.WeightCategoriesListAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
         var response = await _weightCategorieServiceMock.Object.WeightCategoriesListAsync(request);

        // Assert
        _weightCategorieServiceMock.Verify(m => m.WeightCategoriesListAsync(request), Times.Once);
        Assert.Equal(weightCategories.Length, response.Metadata.TotalItemCount);
    }
    
    [Fact]
    public async Task GetSelect2WeightCategorieAsync_ReturnsSelect2Response()
    {
        // Arrange
        var request = new Select2Request()
        {
            PageSize = 10,
            Search = "",
            Skip = 0
        };
        
        var data = WeightCategorieList().Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.WeightName
            })
            .ToArray();

        var expectedResponse = new Select2Response()
        {
            Data = data,
            Total = data.Length
        };

        _weightCategorieServiceMock.Setup(service => service.GetSelect2WeightCategoriesAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
        var response = await _weightCategorieServiceMock.Object.GetSelect2WeightCategoriesAsync(request);

        // Assert
        _weightCategorieServiceMock.Verify(m => m.GetSelect2WeightCategoriesAsync(request), Times.Once);
        Assert.Equal(expectedResponse, response);

    }
    
    private WeightCategorieViewModel[] WeightCategorieList()
    {
        return new[]
        {
             new WeightCategorieViewModel()
            {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 20,
            WeightName = "Биг матсоги",
            AgeGroupName = "Возрастная группа 5-6 лет"
            },
             
            new WeightCategorieViewModel()
            {
                Gender = Gender.Male.MapToString(),
                MaxWeight = 20,
                WeightName = "Биг матсоги",
                AgeGroupName = "Возрастная группа 5-6 лет"
            },
            new WeightCategorieViewModel()
            {
                Gender = Gender.Male.MapToString(),
                MaxWeight = 30,
                WeightName = "Биг матсоги",
                AgeGroupName = "Возрастная группа 7-8лет"
            },
            new WeightCategorieViewModel()
            {
                Gender = Gender.Male.MapToString(),
                MaxWeight = 32,
                WeightName = "Биг матсоги",
                AgeGroupName = "Возрастная группа 5-611 лет"
            },
            new WeightCategorieViewModel()
            {
                Gender = Gender.Male.MapToString(),
                MaxWeight = 34,
                WeightName = "Биг матсоги",
                AgeGroupName = "Возрастная группа 5-6232 лет"
            },
        };
    }
}
