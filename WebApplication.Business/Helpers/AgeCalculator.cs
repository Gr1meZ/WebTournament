using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Business.Helpers
{
    public static class AgeCalculator
    {
        public static int CalculateAge(DateOnly birth) => (DateOnly.FromDateTime(DateTime.Now).DayNumber - birth.DayNumber) / 365;

        public static int CalculateAge(DateTime birth) => DateTime.Now.Subtract(birth).Days / 365;

    }
}
