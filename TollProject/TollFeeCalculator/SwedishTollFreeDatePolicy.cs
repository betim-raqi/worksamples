namespace TollFeeCalculator;

public class SwedishTollFreeDatePolicy : ITollFreeDatePolicy
{
    public bool IsTollFreeDate(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
        if (date.Month == 7) return true;

        bool isTollFree = IsPublicHoliday(date) || IsPublicHoliday(date.AddDays(1));

        return isTollFree;
    }

    private static bool IsPublicHoliday(DateOnly date)
    {
        return GetPublicHolidays(date.Year).Contains(date);
    }

    private static HashSet<DateOnly> GetPublicHolidays(int year)
    {
        DateOnly easterSunday = EasterDates.GetEasterSunday(year);

        HashSet<DateOnly> holidays =
        [
            new(year, 1, 1),                                  // New Year's Day
            new(year, 1, 6),                                  // Epiphany
            easterSunday.AddDays(-2),                         // Good Friday
            easterSunday.AddDays(1),                          // Easter Monday
            new(year, 5, 1),                                  // May Day
            easterSunday.AddDays(39),                         // Ascension Day
            new(year, 6, 6),                                  // National Day
            GetFirstSaturdayFrom(new DateOnly(year, 6, 20)),  // Midsummer Day (Jun 20–26)
            GetFirstSaturdayFrom(new DateOnly(year, 10, 31)), // All Saints' Day (Oct 31–Nov 6)
            new(year, 12, 25),                                // Christmas Day
            new(year, 12, 26),                                // Boxing Day
        ];

        return holidays;
    }

    private static DateOnly GetFirstSaturdayFrom(DateOnly start)
    {
        DateOnly date = start;
        while (date.DayOfWeek != DayOfWeek.Saturday)
        {
            date = date.AddDays(1);
        }
        return date;
    }
}
