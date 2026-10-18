using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests
{
    public class GetTollFeeForSingleDateTests
    {
        [Fact]
        public void GetTollFee_ReturnsCorrectFee_WhenWeekday()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime date = new DateTime(2026, 10, 19, 6, 15, 0);

            int tollFee = tollCalculator.GetTollFee(date, vehicle);

            Assert.Equal(8, tollFee);
        }

        private const int year = 2013;

        private static readonly DateTime Weekday = new(year, 10, 16);

        private static TollCalculator CreateCalculator()
        {
            ITollFreeDateProvider tollFreeDateProvider = new SwedishTollFreeDateProvider();
            ITollFeeSchedule tollFeeSchedule = TollFeeSchedules.Standard;
            ITollFreeVehiclePolicy tollFreeVehiclePolicy = new TollFreeVehiclePolicy();

            return new TollCalculator(tollFreeDateProvider, tollFeeSchedule, tollFreeVehiclePolicy);
        }

        private static DateTime WeekdayAt(int hour, int minute, int second = 0) =>
            Weekday.AddHours(hour).AddMinutes(minute).AddSeconds(second);

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSaturday()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);

            DateTime saturday = new(year,10,19,12,0,0);
            int tollFee = tollCalculator.GetTollFee(saturday, vehicle);

            // Saturday is a toll-free day, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenSunday()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime sunday = new(year, 10, 20, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(sunday, vehicle);

            // Sunday is a toll-free public holiday, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenJuly()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime julyDate = new(year, 7, 15, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(julyDate, vehicle);

            // July is a toll-free month, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenMotorbike()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Motorbike);

            int tollFee = tollCalculator.GetTollFee(WeekdayAt(12, 0), vehicle);

            // Motorbikes are toll-free, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsFee_WhenCar()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);

            int tollFee = tollCalculator.GetTollFee(WeekdayAt(12, 0), vehicle);
            
            // Cars are not toll-free, so the expected toll fee is 8.
            Assert.Equal(8, tollFee);
        }

        [Fact]
        public void GetTollFee_ReturnsZero_WhenPublicHoliday()
        {
            TollCalculator tollCalculator = CreateCalculator();
            IVehicle vehicle = new Vehicle(VehicleKind.Car);
            DateTime date = new(2013, 12, 25, 12, 0, 0);

            int tollFee = tollCalculator.GetTollFee(date, vehicle);

            // Public holidays are toll-free, so the expected toll fee is 0 instead of 8.
            Assert.Equal(0, tollFee);
        }
    }
}
