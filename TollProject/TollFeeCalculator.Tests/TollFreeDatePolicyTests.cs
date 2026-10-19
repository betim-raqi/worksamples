using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests
{
    public class TollFreeDatePolicyTests
    {

        [Theory]
        [MemberData(nameof(GetPublicHolidayDates))]
        public void IsTollFreeDate_ReturnsTrue_WhenPublicHoliday(DateOnly date)
        {
            SwedishTollFreeDatePolicy tollFreeDatePolicy = new();

            bool isTollFree = tollFreeDatePolicy.IsTollFreeDate(date);

            Assert.True(isTollFree);
        }

        [Theory]
        [MemberData(nameof(GetPublicHolidayDates))]
        public void IsTollFreeDate_ReturnsTrue_WhenDayBeforePublicHoliday(DateOnly date)
        {
            SwedishTollFreeDatePolicy tollFreeDatePolicy = new();
            DateOnly dayBefore = date.AddDays(-1);

            bool isTollFree = tollFreeDatePolicy.IsTollFreeDate(dayBefore);
            
            Assert.True(isTollFree);
        }

        public static TheoryData<DateOnly> GetPublicHolidayDates => new()
        {
            { new DateOnly(2013, 1, 1) },
            { new DateOnly(2013, 3, 29) },
            { new DateOnly(2013, 4, 1) },
            { new DateOnly(2013, 5, 1) },
            { new DateOnly(2013, 5, 9) },
            { new DateOnly(2013, 6, 6) },
            { new DateOnly(2013, 6, 22) },
            { new DateOnly(2013, 11, 2) },
            { new DateOnly(2013, 12, 25) },
            { new DateOnly(2013, 12, 26) },

            { new DateOnly(2014, 1, 1) },

            { new DateOnly(2026, 1, 1) },
            { new DateOnly(2026, 1, 6) },
            { new DateOnly(2026, 4, 3) },
            { new DateOnly(2026, 4, 6) },
            { new DateOnly(2026, 5, 1) },
            { new DateOnly(2026, 5, 14) },
            { new DateOnly(2026, 6, 6) },
            { new DateOnly(2026, 6, 20) },
            { new DateOnly(2026, 10, 31) },
            { new DateOnly(2026, 12, 25) },

            { new DateOnly(2027, 1, 1) }
        };
    }
}
