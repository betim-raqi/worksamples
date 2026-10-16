using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForMultipleTollPassesTests
    {
        [Fact]
        public void GetTollFee_ReturnsCorrectTotalFee_WhenTotalIsWithinMaximum()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            int year = 2013;
            DateTime[] dates =
            [
                new (year, 10, 16, 6, 0, 0),   // 06:00 AM - 8 SEK
                new (year, 10, 16, 7, 30, 0),  // 07:30 AM - 18 SEK  
                new (year, 10, 16, 9, 0, 0),   // 09:00 AM - 8 SEK
                new (year, 10, 16, 10, 30, 0), // 10:30 AM - 8 SEK
                new (year, 10, 16, 12, 0, 0),  // 12:00 PM - 8 SEK
                new (year, 10, 16, 13, 30, 0) // Total fee = 8 +18 +8 +8 +8 +8 =58 SEK
            ];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(58, totalFee);
        }

        [Fact]
        public void GetTollFee_ReturnsMaximumFee_WhenTotalExceedsMaximum()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            int year = 2013;
            DateTime[] dates =
            [
                new (year, 10, 16, 6, 0, 0),   // 06:00 AM - 8 SEK
                new (year, 10, 16, 7, 30, 0),  // 07:30 AM - 18 SEK  
                new (year, 10, 16, 9, 0, 0),   // 09:00 AM - 8 SEK
                new (year, 10, 16, 10, 30, 0), // 10:30 AM - 8 SEK
                new (year, 10, 16, 12, 0, 0),  // 12:00 PM - 8 SEK
                new (year, 10, 16, 13, 30, 0), // 13:30 PM - 8 SEK
                new (year, 10, 16, 16, 00, 0)  // 16:00 PM - 18 SEK
            ];

            int maximumFee = 60;
            int feeWithoutCap = dates.Sum(date => tollCalculator.GetTollFee(date, vehicle));

            // Total fee without the cap would be 8 + 18 + 8 + 8 + 8 + 8 + 18 = 76 SEK
            Assert.True(feeWithoutCap > maximumFee);

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            // The total fee should be capped at the maximum of the day (60)
            Assert.Equal(maximumFee, totalFee); 
        }

        [Fact]
        public void GetTollFee_ReturnsHighestToll_WhenMultiplePassesWithinOneHour()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            int year = 2013;
            DateTime[] dates =
            [
                new (year, 10, 16, 6, 15, 0),   // 06:15 AM - 8 SEK
                new (year, 10, 16, 6, 30, 0),  // 06:30 AM - 13 SEK (within one hour of the first pass)
                new (year, 10, 16, 7, 0, 0)    // 07:00 AM - 18 SEK (within one hour of the second pass)
            ];
            int totalFee = tollCalculator.GetTollFee(vehicle, dates);
            // The total fee should be the highest toll within the one-hour window (18 SEK)
            Assert.Equal(18, totalFee);
        }
    }
}
