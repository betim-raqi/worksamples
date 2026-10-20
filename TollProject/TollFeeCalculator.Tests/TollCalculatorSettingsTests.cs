using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Tests;

public class TollCalculatorSettingsTests
{
    [Fact]
    public void Validate_ShouldNotThrowException_WhenIntervalsDoNotOverlap()
    {
        var settings = new TollCalculatorSettings
        {
            MaxDailyFee = 60,
            IntervalWindow = TimeSpan.FromMinutes(60),
            FeeSchedule = [
                new TollFeeInterval(new TimeOnly(8, 0), new TimeOnly(9, 0), 10),
                new TollFeeInterval(new TimeOnly(10, 0), new TimeOnly(11, 0), 20) // Does not overlap with the first interval
            ]
        };
        var exception = Record.Exception(settings.Validate);
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenFeeScheduleIntervalsOverlap()
    {
        var settings = new TollCalculatorSettings
        {
            MaxDailyFee = 60,
            IntervalWindow = TimeSpan.FromMinutes(60),
            FeeSchedule =
            [
                new TollFeeInterval(new TimeOnly(8, 0), new TimeOnly(9, 0), 10),
                new TollFeeInterval(new TimeOnly(8, 30), new TimeOnly(9, 30), 20) // Overlaps with the first interval
            ]
        };

        Assert.Throws<ArgumentException>(settings.Validate);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenFeeOverlapAtBoundary()
    {
        var settings = new TollCalculatorSettings
        {
            MaxDailyFee = 60,
            IntervalWindow = TimeSpan.FromMinutes(60),
            FeeSchedule =
            [
                new TollFeeInterval(new TimeOnly(8, 0), new TimeOnly(9, 0), 10),

                // Overlaps at the boundary with the first interval
                new TollFeeInterval(new TimeOnly(9, 0).Add(TimeSpan.FromTicks(-1)), new TimeOnly(10, 0), 20)
            ]
        };
        Assert.Throws<ArgumentException>(settings.Validate);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenIntervalWindowIsZero()
    {
        var settings = new TollCalculatorSettings
        {
            MaxDailyFee = 60,
            IntervalWindow = TimeSpan.Zero,
            FeeSchedule = [
                new TollFeeInterval(new TimeOnly(8, 0), new TimeOnly(9, 0), 10)
            ]
        };
        Assert.Throws<ArgumentOutOfRangeException>(settings.Validate);
    }

    [Fact]
    public void Validate_ShouldThrowException_WhenMaxDailyFeeIsNegative()
    {
        var settings = new TollCalculatorSettings
        {
            MaxDailyFee = -10,
            IntervalWindow = TimeSpan.FromMinutes(60),
            FeeSchedule = [
                new TollFeeInterval(new TimeOnly(8, 0), new TimeOnly(9, 0), 10)
            ]
        };
        Assert.Throws<ArgumentOutOfRangeException>(settings.Validate);
    }
}
