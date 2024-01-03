namespace WebTournament.Domain.Extensions
{
    public static class AgeCalculator
    {
        public static int CalculateAge(DateTime birth) => DateTime.Now.Subtract(birth).Days / 365;

    }
}
