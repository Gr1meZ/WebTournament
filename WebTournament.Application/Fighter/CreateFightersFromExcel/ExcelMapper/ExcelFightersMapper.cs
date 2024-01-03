using OfficeOpenXml;
using WebTournament.Application.DTO;
using WebTournament.Domain.Exceptions;

namespace WebTournament.Application.Fighter.CreateFightersFromExcel.ExcelMapper;

public class ExcelFightersMapper
{
    public static void ParseExcelFile(ExcelWorksheet? worksheet, int row, FighterDto fighterDto)
    {
          for (var col = 1; col <= 11; col++)
          {
              var cellValue = worksheet?.Cells[row, col].Value;
              var columnHeader = worksheet?.Cells[1, col].Value.ToString();
              if (columnHeader == null || cellValue == null)
                  continue;
                    
              switch (columnHeader)
              {
                  case "Фамилия":
                      fighterDto.Surname = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Имя":
                      fighterDto.Name = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Дата рождения":
                      if (DateTime.TryParse(cellValue.ToString(), out var birthDate))
                          fighterDto.BirthDate = birthDate;
                      else
                          throw new ValidationException("ValidationException", $"Неизвестный формат даты {cellValue}");
                      break;
                  case "Номер пояса":
                      if (int.TryParse(cellValue.ToString(), out var beltNumber))
                          fighterDto.BeltNumber = Convert.ToInt32(beltNumber);
                      else
                          throw new ValidationException("ValidationException", $"Неизвестный формат номера пояса {cellValue}");
                      break;
                  case "Ступень":
                      fighterDto.BeltShortName = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Страна":
                      fighterDto.Country = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Город":
                      fighterDto.City = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Пол":
                      fighterDto.Gender = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Вес":
                      if (int.TryParse(cellValue.ToString(), out var weightNumber))
                          fighterDto.WeightNumber = Convert.ToInt32(weightNumber);
                      else
                          throw new ValidationException("ValidationException", $"Неизвестный формат веса {cellValue}");
                      break;
                  case "Тренер":
                      fighterDto.TrainerName = cellValue.ToString() ?? string.Empty;
                      break;
                  case "Клуб":
                      fighterDto.ClubName = cellValue.ToString() ?? string.Empty;
                      break;
              }
                    
          }  
                
    }
    
}