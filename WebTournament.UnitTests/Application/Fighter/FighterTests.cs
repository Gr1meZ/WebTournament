using AutoMapper;
using CustomExceptionsLibrary;
using Microsoft.AspNetCore.Http;
using Moq;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Application.Fighter;
using WebTournament.Application.Fighter.CreateFightersFromExcel.Validators;
using WebTournament.Application.Fighter.GetFighter;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Fighter;

public class FighterTests
{
    private readonly Mock<IFighterRepository> _fighterRepository = new();
    private readonly Mock<IFormFile> _excelFile = new();
    
    [Fact]
    public async Task GetFighter_Must_BeSuccessful()
    {
        var getFighterQuery = new GetFighterQuery(Guid.NewGuid());
        
        var fighter = await FighterBuilder.BuildAsync(getFighterQuery.Id);

        _fighterRepository.Setup(method => method.GetByIdAsync(getFighterQuery.Id))
            .ReturnsAsync(fighter);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var fighterResponse = mapper.Map<FighterResponse>(fighter);
        
        Assert.Equal(fighter.Id, fighterResponse.Id);
        Assert.Equal(fighter.TournamentId, fighterResponse.TournamentId);
        Assert.Equal(fighter.BeltId, fighterResponse.BeltId);
        Assert.Equal(fighter.TrainerId, fighterResponse.TrainerId);
        Assert.Equal(fighter.WeightCategorieId, fighterResponse.WeightCategorieId);
        Assert.Equal(fighter.Name, fighterResponse.Name);
        Assert.Equal(fighter.Surname, fighterResponse.Surname);
        Assert.Equal(fighter.BirthDate, fighterResponse.BirthDate);
        Assert.Equal(fighter.Country, fighterResponse.Country);
        Assert.Equal(fighter.City, fighterResponse.City);
        Assert.Equal(fighter.Gender.MapToString(), fighterResponse.Gender);
    }
    
    [Fact]
    public void ExcelFile_MustBe_ThrowExceptionWhenLengthIsZero()
    {
        
        _excelFile.Setup(x => x.Length).Returns(0);
        _excelFile.Setup(x => x.FileName).Returns("file.xlsx");
        Assert.Throws<ValidationException>(() => ExcelFileValidator.ValidateFile(_excelFile.Object));
    }
    
    [Fact]
    public void ExcelFile_MustBe_ThrowExceptionWhenFileNameIsNotValid()
    {
        
        _excelFile.Setup(x => x.Length).Returns(5);
        _excelFile.Setup(x => x.FileName).Returns("file.doc");
        Assert.Throws<ValidationException>(() => ExcelFileValidator.ValidateFile(_excelFile.Object));
    }
    
    [Fact]
    public void ExcelFile_MustBe_Valid()
    {
        
        _excelFile.Setup(x => x.Length).Returns(5);
        _excelFile.Setup(x => x.FileName).Returns("file.xlsx");
        ExcelFileValidator.ValidateFile(_excelFile.Object); 
        Assert.True(true);
    }
    
    [Fact]
    public void Worksheet_Must_ThrowExceptionWhenNameNotValid()
    {
        Assert.Throws<ValidationException>(() => ExcelFileValidator.ValidateWorkSheet(default));
    }
}