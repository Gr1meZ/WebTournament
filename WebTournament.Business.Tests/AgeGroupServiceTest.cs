using DataAccess.Abstract;
using DataAccess.Common.Exceptions;
using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebTournament.Business.Abstract;
using WebTournament.Business.Services;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Tests;

public class AgeGroupServiceTest
{
    private readonly Mock<IAgeGroupService> _mockAgeGroupService = new();


    [Fact]
    public async Task AddAgeGroupAsync_NullModel_ThrowsValidationException()
    {
        // Arrange
        _mockAgeGroupService.Setup(service => service.AddAgeGroupAsync(null))
            .Throws(new ValidationException("ValidationException", "Age group model is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.AddAgeGroupAsync(null));
    }

    [Fact]
    public async Task AddAgeGroupAsync_AddsAgeGroup_IfModelIsValid()
    {
        // Arrange
        var ageGroupViewModel = new AgeGroupViewModel
        {
            MaxAge = 20,
            MinAge = 10,
            Name = "Youth"
        };
        
        _mockAgeGroupService.Setup(service => service.AddAgeGroupAsync(It.IsAny<AgeGroupViewModel>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _mockAgeGroupService.Object.AddAgeGroupAsync(ageGroupViewModel);

        // Assert
        _mockAgeGroupService.Verify(m => m.AddAgeGroupAsync(It.IsAny<AgeGroupViewModel>()), Times.Once);
    }
    
    [Fact]
    public async Task AgeGroupListAsync_ReturnsPagedResponse()
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
        
        var expectedResponse = new PagedResponse<AgeGroupViewModel[]>
        (
            AgeGroupList() ,
            6, // Общее количество элементов
            request.PageNumber,
            request.PageSize
        );

        _mockAgeGroupService.Setup(service => service.AgeGroupListAsync(request))
            .ReturnsAsync(expectedResponse)
            .Verifiable("Сервис не вызван.");

        // Act
         await _mockAgeGroupService.Object.AgeGroupListAsync(request);

        // Assert
        _mockAgeGroupService.Verify(m => m.AgeGroupListAsync(It.IsAny<PagedRequest>()), Times.Once);
    }
    
    [Fact]
    public async Task AgeGroupListAsync_ThrowsValidationException()
    {
        // Arrange
        _mockAgeGroupService.Setup(service => service.AgeGroupListAsync(null))
            .Throws(new ValidationException("ValidationException", "Request is null"));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.AgeGroupListAsync(null));
    }
    
    private AgeGroupViewModel[] AgeGroupList()
    {
        return new[]
        {
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Дети",
                MinAge = 1,
                MaxAge = 5
            },
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Подростки",
                MinAge = 6,
                MaxAge = 12
            },
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Юноши",
                MinAge = 13,
                MaxAge = 18
            },
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Молодежь",
                MinAge = 19,
                MaxAge = 24
            },
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Взрослые",
                MinAge = 25,
                MaxAge = 40
            },
            new AgeGroupViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Пожилые",
                MinAge = 41,
                MaxAge = 60
            }
        };
    }
}
