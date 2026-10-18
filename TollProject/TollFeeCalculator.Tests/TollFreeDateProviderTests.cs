using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests
{
    public class TollFreeDateProviderTests
    {

        [Theory]
        [MemberData(nameof(GetPublicHolidayDates))]
        public void IsTollFreeDate_ReturnsTrue_WhenPublicHoliday(DateTime date)
        {
            ITollFreeDateProvider tollFreeDateProvider = new SwedishTollFreeDateProvider();

            bool isTollFree = tollFreeDateProvider.IsTollFreeDate(date);

            Assert.True(isTollFree);
        }

        // TODO: Generalize the public holiday dates for multiple years and add more test cases for different years.
        public static TheoryData<DateTime> GetPublicHolidayDates => new()
        {
            { new DateTime(2013, 1, 1, 12, 0, 0) },
            { new DateTime(2013, 3, 28, 12, 0, 0) },
            { new DateTime(2013, 3, 29, 12, 0, 0) },
            { new DateTime(2013, 4, 1, 12, 0, 0) },
            { new DateTime(2013, 4, 30, 12, 0, 0) },
            { new DateTime(2013, 5, 1, 12, 0, 0) },
            { new DateTime(2013, 5, 8, 12, 0, 0) },
            { new DateTime(2013, 5, 9, 12, 0, 0) },
            { new DateTime(2013, 6, 5, 12, 0, 0) },
            { new DateTime(2013, 6, 6, 12, 0, 0) },
            { new DateTime(2013, 6, 21, 12, 0, 0) },
            { new DateTime(2013, 11, 1, 12, 0, 0) },
            { new DateTime(2013, 12, 24, 12, 0, 0) },
            { new DateTime(2013, 12, 25, 12, 0, 0) },
            { new DateTime(2013, 12, 26, 12, 0, 0) },
            { new DateTime(2013, 12, 31, 12, 0, 0) }
        };
    }
}
