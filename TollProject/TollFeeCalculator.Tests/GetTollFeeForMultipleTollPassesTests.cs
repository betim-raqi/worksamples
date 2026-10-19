using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class GetTollFeeForMultipleTollPassesTests
{
    private static TollCalculator CreateCalculator()
    {
        return new TollCalculator(
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
    public void GetTollFee_ReturnsCorrectTotalFee_WhenTotalIsWithinMaximum()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 00), // 8 SEK
            new (07, 30), // 18 SEK  
            new (09, 00), // 8 SEK
            new (10, 30), // 8 SEK
            new (12, 00), // 8 SEK
            new (13, 30)  // 8 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        Assert.Equal(58, totalFee);
    }

    [Fact]
    public void GetTollFee_ReturnsMaximumFee_WhenTotalExceedsMaximum()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 00), // 8 SEK
            new (07, 30), // 18 SEK  
            new (09, 00), // 8 SEK
            new (10, 30), // 8 SEK
            new (12, 00), // 8 SEK
            new (13, 30), // 8 SEK
            new (16, 00)  // 18 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // The total fee should be capped at the maximum of the day (60)
        Assert.Equal(60, totalFee);
    }

    [Fact]
    public void GetTollFee_ReturnsHighestToll_WhenMultiplePassesWithinOneHourInterval()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 15), // 8 SEK
            new (06, 30), // 13 SEK
            new (07, 00)  // 18 SEK (Highest toll within the one-hour interval)
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // The total fee should be the highest toll within the one-hour interval
        Assert.Equal(18, totalFee);
    }

    [Fact]
    public void GetTollFee_AddsFees_WhenPassesAcrossDifferentOneHourIntervals()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 15), // 8 SEK
            new (06, 30), // 13 SEK
            new (07, 00), // 18 SEK

            new (08, 30), // 8 SEK (new one-hour interval)
            new (09, 00)  // 8 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // Total fee should be the sum of the highest tolls from each one-hour interval
        Assert.Equal(26, totalFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenNoPasses()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime[] dates = []; // No toll passes

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        Assert.Equal(0, totalFee);
    }

    [Fact]
    public void GetTollFee_ReturnsZero_WhenAllPassesAreTollFree()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Motorbike); // Motorbike is a toll-free vehicle
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 00), // 8 SEK
            new (07, 30), // 18 SEK
            new (09, 00)  // 8 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // Toll free vehicles should not incur any toll fees
        Assert.Equal(0, totalFee);
    }

    [Fact]
    public void GetTollFee_AddsNewIntervalFee_WhenAtStartOfNewOneHourInterval()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new TimeOnly(06, 00), // 8 SEK
            new TimeOnly(07, 00).Add(TimeSpan.FromTicks(1)) // 18 SEK (new one-hour interval)
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // The total fee should include both passes since they are in different one-hour intervals
        Assert.Equal(26, totalFee);
    }

    [Fact]
    public void GetTollFee_DoesNotAddFee_WhenPassesAreExactlyOneHourApart()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 00), // 8 SEK
            new (07, 00)  // 18 SEK (exactly one hour later)
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // The total fee should include both passes since they are in different one-hour intervals
        Assert.Equal(18, totalFee);
    }

    [Fact]
    public void GetTollFee_ReturnsCorrectFee_WhenTollPassesUnsorted()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (09, 00), // 8 SEK
            new (06, 00), // 8 SEK
            new (07, 30)  // 18 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        int totalFee = tollCalculator.GetTollFee(vehicle, dates);

        // The total fee should be the same as if the dates were sorted
        Assert.Equal(34, totalFee);
    }

    [Fact]
    public void GetTollFee_ThrowsArgumentNullException_WhenDatesArrayIsNull()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime[] dates = null!; // Intentionally set to null to test exception handling

        Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_ThrowsArgumentNullException_WhenVehicleIsNull()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = null!; // Intentionally set to null to test exception handling
        DateOnly date = new(2013, 10, 16);
        TimeOnly[] times =
        [
            new (06, 00), // 8 SEK
            new (07, 30)  // 18 SEK
        ];
        DateTime[] dates = times.Select(date.ToDateTime).ToArray();

        Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void GetTollFee_ThrowsArgumentException_WhenDatesAcrossMultipleDays()
    {
        TollCalculator tollCalculator = CreateCalculator();
        IVehicle vehicle = new Vehicle(VehicleKind.Car);
        DateTime[] dates =
        [
            new DateTime(2026, 02, 16, 06, 00, 00), // Day 1
            new DateTime(2026, 02, 17, 07, 30, 00)  // Day 2
        ];
        Assert.Throws<ArgumentException>(() => tollCalculator.GetTollFee(vehicle, dates));
    }
}
