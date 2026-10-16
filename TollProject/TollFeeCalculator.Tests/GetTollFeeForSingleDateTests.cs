namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForSingleDateTests
    {
        /// <summary>
        /// 06:00–06:29     8 kr
        /// 06:30–06:59     13 kr
        /// 07:00–07:59     18 kr
        /// 08:00–08:29     13 kr
        /// 08:30–14:59     8 kr
        /// 15:00–15:29     13 kr
        /// 15:30–16:59     18 kr
        /// 17:00–17:59     13 kr
        /// 18:00–18:29     8 kr
        /// 18:30–05:59     0 kr
        /// </summary>
        /// <param name="dateString"></param>
        /// <param name="expectedFee"></param>
        [Theory]
        [InlineData("2026-10-16T06:15:00.000", 8)]
        [InlineData("2026-10-16T06:45:00.000", 13)]
        [InlineData("2026-10-16T07:30:00.000", 18)]
        [InlineData("2026-10-16T08:15:00.000", 13)]
        [InlineData("2026-10-16T09:00:00.000", 8)]
        [InlineData("2026-10-16T15:15:00.000", 13)]
        [InlineData("2026-10-16T15:45:00.000", 18)]
        [InlineData("2026-10-16T17:30:00.000", 13)]
        [InlineData("2026-10-16T18:15:00.000", 8)]
        [InlineData("2026-10-16T18:45:00.000", 0)]
        public void GetTollFee_ReturnsCorrectFee_WhenWeekday(string dateString, int expectedFee)
        {
            TollCalculator tollCalculator = new();
            IVehicle vehicle = new Car();

            DateTime date = DateTime.Parse(dateString);

            int tollFee = tollCalculator.GetTollFee(date, vehicle);
            Assert.Equal(expectedFee, tollFee);
        }
    }
}
