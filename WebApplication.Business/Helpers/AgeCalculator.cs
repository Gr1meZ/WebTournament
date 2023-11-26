namespace WebTournament.Business.Helpers
{
    public static class AgeCalculator
    {
        public static int CalculateAge(DateTime birth) => DateTime.Now.Subtract(birth).Days / 365;

    }
}
