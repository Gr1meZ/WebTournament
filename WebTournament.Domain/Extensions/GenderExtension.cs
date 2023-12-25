using WebTournament.Domain.Enums;

namespace WebTournament.Domain.Extensions
{
    public static class GenderExtension
    {
        public static string MapToString(this Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Мужской",
                Gender.Female => "Женский",
                _ => "Мужской"
            };
        }
        public static Gender ParseEnum(string value)
        {
            return value switch
            {
                "Мужской" => Gender.Male,
                "Женский" => Gender.Female,
                _ => Gender.Male
            };
        }
    }
}
