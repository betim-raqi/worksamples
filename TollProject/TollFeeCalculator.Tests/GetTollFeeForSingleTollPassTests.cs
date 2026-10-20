using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class GetTollFeeForSingleTollPassTests
{
    private readonly TollCalculator _tollCalculator;

    public GetTollFeeForSingleTollPassTests()
    {
        _tollCalculator = new TollCalculator(
            new TollExemptionPolicy(
                new SwedishTollFreeDatePolicy(),
                new TollFreeVehiclePolicy(TollFreeVehicleSets.Standard)),
            new TollCalculatorSettings
            {
                MaxDailyFee = 60,
                IntervalWindow = TimeSpan.FromMinutes(60),
                FeeSchedule = TollFeeSchedules.Standard
            });
    }

    [Fact]
    public void GetTollFee_ReturnsCorrectFee_WhenWeekday()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime date = new DateTime(2026, 10, 19, 6, 15, 0);

        int tollFee = _tollCalculator.GetTollFee(date, vehicle);

        Assert.Equal(8, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenSaturday()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);

        DateTime saturday = new(2013,10,19,12,0,0);
        int tollFee = _tollCalculator.GetTollFee(saturday, vehicle);

        // Saturday is a toll-free day, so the expected toll fee is 0 instead of 8.
        Assert.Equal(0, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenSunday()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime sunday = new(2013, 10, 20, 12, 0, 0);

        int tollFee = _tollCalculator.GetTollFee(sunday, vehicle);

        // Sunday is a toll-free public holiday, so the expected toll fee is 0 instead of 8.
        Assert.Equal(0, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenJuly()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime julyDate = new(2013, 7, 15, 12, 0, 0);

        int tollFee = _tollCalculator.GetTollFee(julyDate, vehicle);

        // July is a toll-free month, so the expected toll fee is 0 instead of 8.
        Assert.Equal(0, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenMotorbike()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Motorbike);
        DateTime date = new(2013, 10, 16, 12, 0, 0);

        int tollFee = _tollCalculator.GetTollFee(date, vehicle);

        // Motorbikes are toll-free, so the expected toll fee is 0 instead of 8.
        Assert.Equal(0, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsFee_WhenCar()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime date = new(2013, 10, 16, 12, 0, 0);

        int tollFee = _tollCalculator.GetTollFee(date, vehicle);
        
        // Cars are not toll-free, so the expected toll fee is 8.
        Assert.Equal(8, tollFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenPublicHoliday()
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime date = new(2013, 12, 25, 12, 0, 0);

        int tollFee = _tollCalculator.GetTollFee(date, vehicle);

        // Public holidays are toll-free, so the expected toll fee is 0 instead of 8.
        Assert.Equal(0, tollFee);
    }

    [Theory]
    [MemberData(nameof(SampleIntervalTimes))]
    public void GetFee_ReturnsExpectedFee(TimeOnly time, int expectedFee)
    {
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime date = new DateOnly(2026, 10, 20).ToDateTime(time); // Regular weekday

        int tollFee = _tollCalculator.GetTollFee(date, vehicle);

        Assert.Equal(expectedFee, tollFee);
    }

    public static TheoryData<TimeOnly, int> SampleIntervalTimes => new()
    {
        { new TimeOnly(6, 0), 8 },
        { new TimeOnly(6, 15), 8 },
        { new TimeOnly(6, 30).Add(TimeSpan.FromTicks(-1)), 8 },

        { new TimeOnly(6, 30), 13 },
        { new TimeOnly(6, 45), 13 },
        { new TimeOnly(7, 0).Add(TimeSpan.FromTicks(-1)), 13 },

        { new TimeOnly(7, 0), 18 },
        { new TimeOnly(7, 30), 18 },
        { new TimeOnly(8, 0).Add(TimeSpan.FromTicks(-1)), 18 },

        { new TimeOnly(8, 0), 13 },
        { new TimeOnly(8, 15), 13 },
        { new TimeOnly(8, 30).Add(TimeSpan.FromTicks(-1)), 13 },

        { new TimeOnly(8, 30), 8 },
        { new TimeOnly(9, 0), 8 },
        { new TimeOnly(15, 0).Add(TimeSpan.FromTicks(-1)), 8 },

        { new TimeOnly(15, 0), 13 },
        { new TimeOnly(15, 15), 13 },
        { new TimeOnly(15, 30).Add(TimeSpan.FromTicks(-1)), 13 },

        { new TimeOnly(15, 30), 18 },
        { new TimeOnly(16, 0), 18 },
        { new TimeOnly(17, 0).Add(TimeSpan.FromTicks(-1)), 18 },

        { new TimeOnly(17, 0), 13 },
        { new TimeOnly(17, 30), 13 },
        { new TimeOnly(18, 0).Add(TimeSpan.FromTicks(-1)), 13 },

        { new TimeOnly(18, 0), 8 },
        { new TimeOnly(18, 15), 8 },
        { new TimeOnly(18, 30).Add(TimeSpan.FromTicks(-1)), 8 },

        { new TimeOnly(18, 30), 0 },
        { new TimeOnly(2, 0), 0 },
        { new TimeOnly(6, 0).Add(TimeSpan.FromTicks(-1)), 0 }
    };
}
