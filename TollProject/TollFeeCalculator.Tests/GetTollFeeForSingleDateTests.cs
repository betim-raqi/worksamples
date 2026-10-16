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
            { new DateTime(2026,10,16,06,15,0), 8, "06:00–06:29" },
            { new DateTime(2026,10,16,06,45,0), 13, "06:30–06:59"  },
            { new DateTime(2026,10,16,07,30,0), 18, "07:00–07:59" },
            { new DateTime(2026,10,16,08,15,0), 13, "08:00–08:29" },
            { new DateTime(2026,10,16,09,0,0), 8, "08:30–14:59" },
            { new DateTime(2026,10,16,15,15,0), 13, "15:00–15:29" },
            { new DateTime(2026,10,16,15,45,0), 18, "15:30–16:59" },
            { new DateTime(2026,10,16,17,30,0), 13, "17:00–17:59" },
            { new DateTime(2026,10,16,18,15,0), 8, "18:00–18:29" },
            { new DateTime(2026,10,16,18,45,0), 0, "18:30–05:59" }
        };
    }
}
