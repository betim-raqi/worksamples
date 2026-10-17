using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public class SwedishTollFreeDateProvider : ITollFreeDateProvider
    {
        public bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            if (date.Month == 7) return true;

            bool isTollFree = IsPublicHoliday(date) || IsPublicHoliday(date.AddDays(1));

            return isTollFree;
        }

        private static bool IsPublicHoliday(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||                    // New Year's Day
                    month == 3 && day == 29 ||                   // Good Friday
                    month == 4 && day == 1 ||                    // Easter Monday
                    month == 5 && (day == 1 || day == 9) ||      // May Day, Ascension Day
                    month == 6 && (day == 6 || day == 22) ||     // National Day, Midsummer Day
                    month == 11 && day == 2 ||                   // All Saints' Day
                    month == 12 && (day == 25 || day == 26))     // Christmas Day, Boxing Day
                {
                    return true;
                }
            }

            // TODO: Change temporary FIX! Needed so Dec 31, 2013 counts as the day before a holiday.
            if (year == 2014 && month == 1 && day == 1)          // New Year's Day
            {
                return true;
            }

            return false;
        }
    }
}
