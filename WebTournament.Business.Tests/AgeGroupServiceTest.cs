// using Moq;
// using WebTournament.Models.Helpers;
//
// namespace WebTournament.Business.Tests;
//
// public class AgeGroupServiceTest
// {
//     private readonly Mock<IAgeGroupService> _mockAgeGroupService = new();
//
//
//     [Fact]
//     public async Task AddAgeGroupAsync_NullModel_ThrowsValidationException()
//     {
//         // Arrange
//         _mockAgeGroupService.Setup(service => service.AddAgeGroupAsync(null))
//             .Throws(new ValidationException("ValidationException", "Age group model is null"));
//
//         // Act & Assert
//         await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.AddAgeGroupAsync(null));
//     }
//
//     [Fact]
//     public async Task AddAgeGroupAsync_AddsAgeGroup_IfModelIsValid()
//     {
//         // Arrange
//         var ageGroupViewModel = new AgeGroupViewModel
//         {
//             MaxAge = 20,
//             MinAge = 10,
//             Name = "Youth"
//         };
//         
//         _mockAgeGroupService.Setup(service => service.AddAgeGroupAsync(It.IsAny<AgeGroupViewModel>()))
//             .Returns(Task.CompletedTask)
//             .Verifiable();
//
//         // Act
//         await _mockAgeGroupService.Object.AddAgeGroupAsync(ageGroupViewModel);
//
//         // Assert
//         _mockAgeGroupService.Verify(m => m.AddAgeGroupAsync(ageGroupViewModel), Times.Once);
//     }
//     
//     [Fact]
//     public async Task DeleteAgeGroupAsync_DeleteIfFound()
//     {
//         // Arrange
//         var ageGroupId = new Guid();
//         
//         _mockAgeGroupService.Setup(service => service.DeleteAgeGroupAsync(It.IsAny<Guid>()))
//             .Returns(Task.CompletedTask)
//             .Verifiable();
//
//         // Act
//         await _mockAgeGroupService.Object.DeleteAgeGroupAsync(ageGroupId);
//
//         // Assert
//         _mockAgeGroupService.Verify(m => m.DeleteAgeGroupAsync(ageGroupId), Times.Once);
//     }
//     
//     [Fact]
//     public async Task DeleteAgeGroupAsync_IfAgeGroupNotFound()
//     {
//         // Arrange
//         _mockAgeGroupService.Setup(service => service.DeleteAgeGroupAsync(Guid.Empty))
//             .Throws(new ValidationException("ValidationException","Age group not found"));
//
//         // Act & Assert
//         await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.DeleteAgeGroupAsync(Guid.Empty));
//     }
//     
//     [Fact]
//     public async Task EditAgeGroupAsync_IfNullModel()
//     {
//         // Arrange
//         _mockAgeGroupService.Setup(service => service.EditAgeGroupAsync(null))
//             .Throws(new ValidationException("ValidationException","Age group not found"));
//
//         // Act & Assert
//         await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.EditAgeGroupAsync(null));
//     }
//     
//     [Fact]
//     public async Task EditAgeGroupAsync_IfModelValid()
//     {
//         // Arrange
//         var ageGroupViewModel = new AgeGroupViewModel
//         {
//             Id = new Guid(),
//             MaxAge = 20,
//             MinAge = 10,
//             Name = "Youth"
//         };
//         
//         _mockAgeGroupService.Setup(service => service.EditAgeGroupAsync(It.IsAny<AgeGroupViewModel>()))
//             .Returns(Task.CompletedTask)
//             .Verifiable();
//
//         // Act
//         await _mockAgeGroupService.Object.EditAgeGroupAsync(ageGroupViewModel);
//
//         // Assert
//         _mockAgeGroupService.Verify(m => m.EditAgeGroupAsync(ageGroupViewModel), Times.Once);
//     }
//     
//     [Fact]
//     public async Task GetAgeGroupAsync_IfAgeGroupNotFound()
//     {
//         // Arrange
//         _mockAgeGroupService.Setup(service => service.GetAgeGroupAsync(Guid.Empty))
//             .Throws(new ValidationException("ValidationException","Age group not found"));
//
//         // Act & Assert
//         await Assert.ThrowsAsync<ValidationException>(() => _mockAgeGroupService.Object.GetAgeGroupAsync(Guid.Empty));
//     }
//     
//     [Fact]
//     public async Task GetAgeGroupAsync_IfAgeGroupIsFounded()
//     {
//         // Arrange
//         var id = Guid.NewGuid();
//         var ageGroupViewModel = new AgeGroupViewModel()
//         {
//             Id = id,
//             Name = "Youth",
//             MinAge = 15,
//             MaxAge = 20
//         };
//         
//         _mockAgeGroupService.Setup(service => service.GetAgeGroupAsync(It.Is<Guid>(x => x != Guid.Empty)))
//             .ReturnsAsync(() => ageGroupViewModel);
//         
//         var result = await _mockAgeGroupService.Object.GetAgeGroupAsync(id);
//         // Act & Assert
//         Assert.Equal(result.Id, id);
//     }
//     
//     [Fact]
//     public async Task AgeGroupListAsync_ReturnsPagedResponse()
//     {
//         // Arrange
//         var request = new PagedRequest
//         {
//             PageNumber = 1,
//             PageSize = 10,
//             Search = "",
//             OrderColumn = "test",
//             OrderDir = "asc"
//         };
//         var ageGroups = AgeGroupList();
//         var expectedResponse = new PagedResponse<AgeGroupViewModel[]>
//         (
//             ageGroups,
//             6, // Общее количество элементов
//             request.PageNumber,
//             request.PageSize
//         );
//
//         _mockAgeGroupService.Setup(service => service.AgeGroupListAsync(request))
//             .ReturnsAsync(expectedResponse)
//             .Verifiable("Сервис не вызван.");
//
//         // Act
//          var response = await _mockAgeGroupService.Object.AgeGroupListAsync(request);
//
//         // Assert
//         _mockAgeGroupService.Verify(m => m.AgeGroupListAsync(request), Times.Once);
//         Assert.Equal(ageGroups.Length, response.Metadata.TotalItemCount);
//
//     }
//     
//     [Fact]
//     public async Task GetSelect2AgeGroupsAsync_ReturnsSelect2Response()
//     {
//         // Arrange
//         var request = new Select2Request()
//         {
//             PageSize = 10,
//             Search = "",
//             Skip = 0
//         };
//         
//         var data = AgeGroupList().Select(x => new Select2Data()
//             {
//                 Id = x.Id,
//                 Name = x.Name
//             })
//             .ToArray();
//
//         var expectedResponse = new Select2Response()
//         {
//             Data = data,
//             Total = data.Length
//         };
//
//         _mockAgeGroupService.Setup(service => service.GetSelect2AgeGroupsAsync(request))
//             .ReturnsAsync(expectedResponse)
//             .Verifiable("Сервис не вызван.");
//
//         // Act
//         var response = await _mockAgeGroupService.Object.GetSelect2AgeGroupsAsync(request);
//
//         // Assert
//         _mockAgeGroupService.Verify(m => m.GetSelect2AgeGroupsAsync(request), Times.Once);
//         Assert.Equal(expectedResponse, response);
//
//     }
//     
//     private AgeGroupViewModel[] AgeGroupList()
//     {
//         return new[]
//         {
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Дети",
//                 MinAge = 1,
//                 MaxAge = 5
//             },
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Подростки",
//                 MinAge = 6,
//                 MaxAge = 12
//             },
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Юноши",
//                 MinAge = 13,
//                 MaxAge = 18
//             },
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Молодежь",
//                 MinAge = 19,
//                 MaxAge = 24
//             },
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Взрослые",
//                 MinAge = 25,
//                 MaxAge = 40
//             },
//             new AgeGroupViewModel
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Пожилые",
//                 MinAge = 41,
//                 MaxAge = 60
//             }
//         };
//     }
// }
