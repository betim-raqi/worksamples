namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForSingleDateTests
    {
        [Theory]
        [MemberData(nameof(GetDates))]
        public void GetTollFee_ReturnsCorrectFee_WhenWeekday(DateTime date, int expectedFee, string caseName)
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();

            int tollFee = tollCalculator.GetTollFee(date, vehicle);
            Assert.Equal(expectedFee, tollFee);
        }

        public static TheoryData<DateTime, int, string> GetDates => new()
        {
            { new DateTime(2026,10,16,06,00,00), 8, "06:00–06:29 (inclusive start)" },
            { new DateTime(2026,10,16,06,15,00), 8, "06:00–06:29 (within)" },
            { new DateTime(2026,10,16,06,30,00).AddTicks(-1), 8, "06:00–06:29 (exclusive end)" },

            { new DateTime(2026,10,16,06,30,00), 13, "06:30–06:59 (inclusive start)" },
            { new DateTime(2026,10,16,06,45,00), 13, "06:30–06:59 (within)" },
            { new DateTime(2026,10,16,07,0,00).AddTicks(-1), 13, "06:30–06:59 (exclusive end)" },

            { new DateTime(2026,10,16,07,0,00), 18, "07:00–07:59 (inclusive start)" },
            { new DateTime(2026,10,16,07,30,00), 18, "07:00–07:59 (within)" },
            { new DateTime(2026,10,16,08,00,00).AddTicks(-1), 18, "07:00–07:59 (exclusive end)" },

            { new DateTime(2026,10,16,08,00,00), 13, "08:00–08:29 (inclusive start)" },
            { new DateTime(2026,10,16,08,15,00), 13, "08:00–08:29 (within)" },
            { new DateTime(2026,10,16,08,30,00).AddTicks(-1), 13, "08:00–08:29 (exclusive end)" },

            { new DateTime(2026,10,16,08,30,00), 8, "08:30–14:59 (inclusive start)" },
            { new DateTime(2026,10,16,09,00,00), 8, "08:30–14:59 (within)" },
            { new DateTime(2026,10,16,15,00,00).AddTicks(-1), 8, "08:30–14:59 (exclusive end)" },

            { new DateTime(2026,10,16,15,00,00), 13, "15:00–15:29 (inclusive start)" },
            { new DateTime(2026,10,16,15,15,00), 13, "15:00–15:29 (within)" },
            { new DateTime(2026,10,16,15,30,00).AddTicks(-1), 13, "15:00–15:29 (exclusive end)" },

            { new DateTime(2026,10,16,15,30,00), 18, "15:30–16:59 (inclusive start)" },
            { new DateTime(2026,10,16,16,00,00), 18, "15:30–16:59 (within)" },
            { new DateTime(2026,10,16,17,00,00).AddTicks(-1), 18, "15:30–16:59 (exclusive end)" },

            { new DateTime(2026,10,16,17,00,00), 13, "17:00–17:59 (inclusive start)" },
            { new DateTime(2026,10,16,17,30,00), 13, "17:00–17:59 (within)" },
            { new DateTime(2026,10,16,18,00,00).AddTicks(-1), 13, "17:00–17:59 (exclusive end)" },

            { new DateTime(2026,10,16,18,00,00), 8, "18:00–18:29 (inclusive start)" },
            { new DateTime(2026,10,16,18,15,00), 8, "18:00–18:29 (within)" },
            { new DateTime(2026,10,16,18,30,00).AddTicks(-1), 8, "18:00–18:29 (exclusive end)" },

            { new DateTime(2026,10,16,18,30,00), 0, "18:30–05:59 (inclusive start)" },
            { new DateTime(2026,10,16,02,00,00), 0, "18:30–05:59 (within)" },
            { new DateTime(2026,10,16,06,00,00).AddTicks(-1), 0, "18:30–05:59 (exclusive end)" }
        };

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSaturday()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();

            DateTime saturday = new(2026,10,17,12,0,0);
            int tollFee = tollCalculator.GetTollFee(saturday, vehicle);

            // Saturday is a toll-free day, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSunday()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            DateTime sunday = new(2026, 10, 18, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(sunday, vehicle);

            // Sunday is a toll-free public holiday, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenJuly()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            DateTime julyDate = new(2026, 7, 15, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(julyDate, vehicle);

            // July is a toll-free month, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenMotorbike()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Motorbike();
            DateTime nonTollfreeDate = new(2026, 10, 16, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(nonTollfreeDate, vehicle);

            // Motorbikes are toll-free, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsFee_WhenCar()
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();
            DateTime nonTollfreeDate = new(2026, 10, 16, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(nonTollfreeDate, vehicle);
            
            // Cars are not toll-free, so the expected toll fee is 8.
            Assert.Equal(8, tollFee);
        }
    }
}
