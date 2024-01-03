using CustomExceptionsLibrary;

namespace WebTournament.Application.Bracket.Validators;

public class BracketValidator
{
    public static void ValidateAgeGroup(Guid ageGroupId)
    {
        if (ageGroupId == Guid.Empty)
        {
            throw new ValidationException("ValidationException", "Не выбрана возрастная группа!");
        }
    }
    public static void IsFighterHaveBracket(Guid bracketId, string surname, string name)
    {
        if (bracketId == Guid.Empty)
        {
            throw new ValidationException("ValidationException", $"Не найдена подходящая сетка для спортсмена {surname} {name}. Создайте все необходимые сетки перед жеребьевкой!");
        }
    }
}