using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class GetTollFeeForSingleDateTests
{
    private readonly TollCalculator _tollCalculator;

    public GetTollFeeForSingleDateTests()
    {
        _tollCalculator = new TollCalculator(
            new SwedishTollFreeDatePolicy(),
            TollFeeSchedules.Standard,
            new TollFreeVehiclePolicy(),
            new TollCalculatorSettings
            {
                MaxDailyFee = 60,
                IntervalWindow = TimeSpan.FromMinutes(60)
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
}
