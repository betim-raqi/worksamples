using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests
{
    public class TollFreeDatePolicyTests
    {

        [Theory]
        [MemberData(nameof(GetPublicHolidayDates))]
        public void IsTollFreeDate_ReturnsTrue_WhenPublicHoliday(DateTime date)
        {
            ITollFreeDatePolicy tollFreeDatePolicy = new SwedishTollFreeDatePolicy();

            bool isTollFree = tollFreeDatePolicy.IsTollFreeDate(date);

            Assert.True(isTollFree);
        }

        [Theory]
        [MemberData(nameof(GetPublicHolidayDates))]
        public void IsTollFreeDate_ReturnsTrue_WhenDayBeforePublicHoliday(DateTime date)
        {
            ITollFreeDatePolicy tollFreeDatePolicy = new SwedishTollFreeDatePolicy();
            DateTime dayBefore = date.AddDays(-1);

            bool isTollFree = tollFreeDatePolicy.IsTollFreeDate(dayBefore);
            
            Assert.True(isTollFree);
        }

        public static TheoryData<DateTime> GetPublicHolidayDates => new()
        {
            { new DateTime(2013, 1, 1, 12, 0, 0) },
            { new DateTime(2013, 3, 29, 12, 0, 0) },
            { new DateTime(2013, 4, 1, 12, 0, 0) },
            { new DateTime(2013, 5, 1, 12, 0, 0) },
            { new DateTime(2013, 5, 9, 12, 0, 0) },
            { new DateTime(2013, 6, 6, 12, 0, 0) },
            { new DateTime(2013, 6, 22, 12, 0, 0) },
            { new DateTime(2013, 11, 2, 12, 0, 0) },
            { new DateTime(2013, 12, 25, 12, 0, 0) },
            { new DateTime(2013, 12, 26, 12, 0, 0) },

            { new DateTime(2014, 1, 1, 12, 0, 0) },

            { new DateTime(2026, 1, 1, 12, 0, 0) },
            { new DateTime(2026, 1, 6, 12, 0, 0) },
            { new DateTime(2026, 4, 3, 12, 0, 0) },
            { new DateTime(2026, 4, 6, 12, 0, 0) },
            { new DateTime(2026, 5, 1, 12, 0, 0) },
            { new DateTime(2026, 5, 14, 12, 0, 0) },
            { new DateTime(2026, 6, 6, 12, 0, 0) },
            { new DateTime(2026, 6, 20, 12, 0, 0) },
            { new DateTime(2026, 10, 31, 12, 0, 0) },
            { new DateTime(2026, 12, 25, 12, 0, 0) },

            { new DateTime(2027, 1, 1, 12, 0, 0) }
        };
    }
}
