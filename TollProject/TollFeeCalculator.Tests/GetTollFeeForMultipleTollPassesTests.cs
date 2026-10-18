using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForMultipleTollPassesTests
    {
        private const int year = 2013;
        private static readonly DateTime Weekday = new(year, 10, 16);
        private static DateTime WeekdayAt(int hour, int minute, int second = 0) =>
            Weekday.AddHours(hour).AddMinutes(minute).AddSeconds(second);
        private TollCalculator CreteCalculator() => new TollCalculator(new SwedishTollFreeDateProvider(), new StandardTollFeeSchedule(), new TollFreeVehiclePolicy());

        [Fact]
        public void GetTollFee_ReturnsCorrectTotalFee_WhenTotalIsWithinMaximum()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(6, 0),   // 06:00 AM - 8 SEK
                WeekdayAt(7, 30),  // 07:30 AM - 18 SEK  
                WeekdayAt(9, 0),   // 09:00 AM - 8 SEK
                WeekdayAt(10, 30), // 10:30 AM - 8 SEK
                WeekdayAt(12, 0),  // 12:00 PM - 8 SEK
                WeekdayAt(13, 30) // Total fee = 8 +18 +8 +8 +8 +8 =58 SEK
            ];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(58, totalFee);
        }

        [Fact]
        public void GetTollFee_ReturnsMaximumFee_WhenTotalExceedsMaximum()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(6, 0),   // 06:00 AM - 8 SEK
                WeekdayAt(7, 30),  // 07:30 AM - 18 SEK  
                WeekdayAt(9, 0),   // 09:00 AM - 8 SEK
                WeekdayAt(10, 30), // 10:30 AM - 8 SEK
                WeekdayAt(12, 0),  // 12:00 PM - 8 SEK
                WeekdayAt(13, 30), // 13:30 PM - 8 SEK
                WeekdayAt(16, 0)  // 16:00 PM - 18 SEK
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
        public void GetTollFee_ReturnsHighestToll_WhenMultiplePassesWithinOneHourInterval()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(6, 15),   // 06:15 AM - 8 SEK
                WeekdayAt(6, 30),  // 06:30 AM - 13 SEK
                WeekdayAt(7, 0)    // 07:00 AM - 18 SEK
            ];
            int totalFee = tollCalculator.GetTollFee(vehicle, dates);
            // The total fee should be the highest toll within the one-hour interval (18 SEK)
            Assert.Equal(18, totalFee);
        }

        [Fact]
        public void GetTollFee_AddsFees_WhenPassesAcrossDifferentOneHourIntervals()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(6, 15),   // 06:15 AM - 8 SEK
                WeekdayAt(6, 30),  // 06:30 AM - 13 SEK
                WeekdayAt(7, 0),    // 07:00 AM - 18 SEK

                WeekdayAt(8, 30),   // 08:30 AM - 8 SEK (new one-hour interval)
                WeekdayAt(9, 0)     // 09:00 AM - 8 SEK
            ];
            int totalFee = tollCalculator.GetTollFee(vehicle, dates);
            // The total fee should be the sum of the highest tolls from each one-hour interval (18 + 8 = 26 SEK)
            Assert.Equal(26, totalFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenNoPasses()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates = [];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(0, totalFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenAllPassesAreTollFree()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Motorbike); // Motorbike is a toll-free vehicle
            DateTime[] dates =
            [
                WeekdayAt(6, 0),   // 06:00 AM - 8 SEK (but vehicle is toll-free)
                WeekdayAt(7, 30),  // 07:30 AM - 18 SEK (but vehicle is toll-free)
                WeekdayAt(9, 0)    // 09:00 AM - 8 SEK (but vehicle is toll-free)
            ];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(0, totalFee);
        }

        [Fact]
        public void GetTollFee_AddsNewIntervalFee_WhenAtStartOfNewOneHourInterval()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            // TODO: Consider using higher precision for the time to ensure that the new interval is correctly identified.
            DateTime[] dates =
            [
                WeekdayAt(6, 0),
                WeekdayAt(7, 1) // 07:01 AM - 18 SEK (new one-hour interval)
            ];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            // The total fee should include both passes since they are in different one-hour intervals (8 + 18 = 26 SEK)
            Assert.Equal(26, totalFee);
        }

        [Fact]
        public void GetTollFee_DoesNotAddFee_WhenPassesAreExactlyOneHourApart()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(6, 0),   // 06:00 AM - 8 SEK
                WeekdayAt(7, 0)    // 07:00 AM - 18 SEK (exactly one hour later)
            ];
            int totalFee = tollCalculator.GetTollFee(vehicle, dates);
            // The total fee should include both passes since they are in different one-hour intervals (8 + 18 = 26 SEK)
            Assert.Equal(18, totalFee);
        }

        [Fact]
        public void GetTollFee_ReturnsCorrectFee_WhenTollPassesUnsorted()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates =
            [
                WeekdayAt(9, 0),   // 09:00 AM - 8 SEK
                WeekdayAt(6, 0),   // 06:00 AM - 8 SEK
                WeekdayAt(7, 30)   // 07:30 AM - 18 SEK
            ];

            int totalFee = tollCalculator.GetTollFee(vehicle, dates);

            // The total fee should be the same as if the dates were sorted (8 + 18 + 8 = 34 SEK)
            Assert.Equal(34, totalFee);
        }

        [Fact]
        public void GetTollFee_ThrowsArgumentNullException_WhenDatesArrayIsNull()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime[] dates = null!;
            Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(vehicle, dates));
        }

        [Fact]
        public void GetTollFee_ThrowsArgumentNullException_WhenVehicleIsNull()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = null!;
            DateTime[] dates =
            [
                WeekdayAt(6, 0),
                WeekdayAt(7, 30)
            ];
            Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(vehicle, dates));
        }
    }
}
