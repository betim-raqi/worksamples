namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForSingleDateTests
    {
        [Theory]
        [MemberData(nameof(GetDates))]
        public void GetTollFee_ReturnsCorrectFee_WhenWeekday(DateTime date, int expectedFee, string caseName)
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();

            int tollFee = tollCalculator.GetTollFee(date, vehicle);
            Assert.Equal(expectedFee, tollFee);
        }

        private const int year = 2013;

        private static readonly DateTime Weekday = new(year, 10, 16);
        private TollCalculator CreteCalculator() => new TollCalculator(new SwedishTollFreeDateProvider());

        private static DateTime WeekdayAt(int hour, int minute, int second = 0) =>
            Weekday.AddHours(hour).AddMinutes(minute).AddSeconds(second);

        public static TheoryData<DateTime, int, string> GetDates => new()
        {
            { WeekdayAt(6, 0), 8, "06:00–06:29 (inclusive start)" },
            { WeekdayAt(6, 15), 8, "06:00–06:29 (within)" },
            { WeekdayAt(6, 30).AddTicks(-1), 8, "06:00–06:29 (exclusive end)" },

            { WeekdayAt(6, 30), 13, "06:30–06:59 (inclusive start)" },
            { WeekdayAt(6, 45), 13, "06:30–06:59 (within)" },
            { WeekdayAt(7, 0).AddTicks(-1), 13, "06:30–06:59 (exclusive end)" },

            { WeekdayAt(7, 0), 18, "07:00–07:59 (inclusive start)" },
            { WeekdayAt(7, 30), 18, "07:00–07:59 (within)" },
            { WeekdayAt(8, 0).AddTicks(-1), 18, "07:00–07:59 (exclusive end)" },

            { WeekdayAt(8, 0), 13, "08:00–08:29 (inclusive start)" },
            { WeekdayAt(8, 15), 13, "08:00–08:29 (within)" },
            { WeekdayAt(8, 30).AddTicks(-1), 13, "08:00–08:29 (exclusive end)" },

            { WeekdayAt(8, 30), 8, "08:30–14:59 (inclusive start)" },
            { WeekdayAt(9, 0), 8, "08:30–14:59 (within)" },
            { WeekdayAt(15, 0).AddTicks(-1), 8, "08:30–14:59 (exclusive end)" },

            { WeekdayAt(15, 0), 13, "15:00–15:29 (inclusive start)" },
            { WeekdayAt(15, 15), 13, "15:00–15:29 (within)" },
            { WeekdayAt(15, 30).AddTicks(-1), 13, "15:00–15:29 (exclusive end)" },

            { WeekdayAt(15, 30), 18, "15:30–16:59 (inclusive start)" },
            { WeekdayAt(16, 0), 18, "15:30–16:59 (within)" },
            { WeekdayAt(17, 0).AddTicks(-1), 18, "15:30–16:59 (exclusive end)" },

            { WeekdayAt(17, 0), 13, "17:00–17:59 (inclusive start)" },
            { WeekdayAt(17, 30), 13, "17:00–17:59 (within)" },
            { WeekdayAt(18, 0).AddTicks(-1), 13, "17:00–17:59 (exclusive end)" },

            { WeekdayAt(18, 0), 8, "18:00–18:29 (inclusive start)" },
            { WeekdayAt(18, 15), 8, "18:00–18:29 (within)" },
            { WeekdayAt(18, 30).AddTicks(-1), 8, "18:00–18:29 (exclusive end)" },

            { WeekdayAt(18, 30), 0, "18:30–05:59 (inclusive start)" },
            { WeekdayAt(2, 0), 0, "18:30–05:59 (within)" },
            { WeekdayAt(6, 0).AddTicks(-1), 0, "18:30–05:59 (exclusive end)" }
        };

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSaturday()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();

            DateTime saturday = new(year,10,19,12,0,0);
            int tollFee = tollCalculator.GetTollFee(saturday, vehicle);

            // Saturday is a toll-free day, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSunday()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();
            DateTime sunday = new(year, 10, 20, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(sunday, vehicle);

            // Sunday is a toll-free public holiday, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenJuly()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();
            DateTime julyDate = new(year, 7, 15, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(julyDate, vehicle);

            // July is a toll-free month, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenMotorbike()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Motorbike();

            int tollFee = tollCalculator.GetTollFee(WeekdayAt(12, 0), vehicle);

            // Motorbikes are toll-free, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsFee_WhenCar()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();

            int tollFee = tollCalculator.GetTollFee(WeekdayAt(12, 0), vehicle);
            
            // Cars are not toll-free, so the expected toll fee is 8.
            Assert.Equal(8, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenPublicHoliday()
        {
            TollCalculator tollCalculator = CreteCalculator();
            IVehicle vehicle = new Car();
            DateTime date = new(2013, 12, 25, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(date, vehicle);

            // Public holidays are toll-free, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }
    }
}
