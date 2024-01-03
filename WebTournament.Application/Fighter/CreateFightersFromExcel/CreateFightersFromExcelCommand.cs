using Microsoft.AspNetCore.Http;
using WebTournament.Application.Configuration.Commands;

namespace WebTournament.Application.Fighter.CreateFightersFromExcel;

public class CreateFightersFromExcelCommand : ICommand
{
    public CreateFightersFromExcelCommand(Guid tournamentId, IFormFile excelFile)
    {
        Id = tournamentId;
        ExcelFile = excelFile;
    }

    public Guid Id { get; }
    public IFormFile ExcelFile { get; }
}