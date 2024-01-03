using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using WebTournament.Domain.Exceptions;

namespace WebTournament.Application.Fighter.CreateFightersFromExcel.Validators;

public  class ExcelFileValidator
{
    public static void ValidateFile(IFormFile excelFile)
    {
        if (excelFile is not { Length: > 0 })
            throw new ValidationException("ValidationException", "Файл пустой");

        if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("ValidationException", "Не поддерживаемый формат файла");
    }
    
    public static void ValidateWorkSheet(ExcelWorksheet? worksheet)
    {
        if (worksheet?.Name != "Заявка")
            throw new ValidationException("ValidationException", "Данный excel файл не содержит заявку на участников. Пожалуйста, добавьте правильную заявку!");
        RemoveEmptyRows(worksheet);
    }
    private static void RemoveEmptyRows(ExcelWorksheet worksheet)
    {
        for (var row = worksheet.Dimension.End.Row; row >= 1; row--)
        {
            var allEmpty = true;
            for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Cells[row, col].Value == null ||
                    string.IsNullOrEmpty(worksheet.Cells[row, col].Value.ToString())) continue;
                allEmpty = false;
                break;
            }

            if (allEmpty)
            {
                worksheet.DeleteRow(row);
            }
        }
    }
}