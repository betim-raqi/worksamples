namespace TollFeeCalculator.Tests;

public class StandardTollFeeScheduleTests
{
    private static TimeOnly At(int hour, int minute, int second = 0) =>
        new(hour, minute, second);
    private static TimeOnly JustBefore(int hour, int minute, int second = 0) =>
        new TimeOnly(hour, minute, second).Add(TimeSpan.FromTicks(-1));

    public static TheoryData<TimeOnly, int, string> SampleIntervalTimes => new()
    {
        { At(6, 0), 8, "06:00–06:29 (inclusive start)" },
        { At(6, 15), 8, "06:00–06:29 (within)" },
        { JustBefore(6, 30), 8, "06:00–06:29 (exclusive end)" },

        { At(6, 30), 13, "06:30–06:59 (inclusive start)" },
        { At(6, 45), 13, "06:30–06:59 (within)" },
        { JustBefore(7, 0), 13, "06:30–06:59 (exclusive end)" },

        { At(7, 0), 18, "07:00–07:59 (inclusive start)" },
        { At(7, 30), 18, "07:00–07:59 (within)" },
        { JustBefore(8, 0), 18, "07:00–07:59 (exclusive end)" },

        { At(8, 0), 13, "08:00–08:29 (inclusive start)" },
        { At(8, 15), 13, "08:00–08:29 (within)" },
        { JustBefore(8, 30), 13, "08:00–08:29 (exclusive end)" },

        { At(8, 30), 8, "08:30–14:59 (inclusive start)" },
        { At(9, 0), 8, "08:30–14:59 (within)" },
        { JustBefore(15, 0), 8, "08:30–14:59 (exclusive end)" },

        { At(15, 0), 13, "15:00–15:29 (inclusive start)" },
        { At(15, 15), 13, "15:00–15:29 (within)" },
        { JustBefore(15, 30), 13, "15:00–15:29 (exclusive end)" },

        { At(15, 30), 18, "15:30–16:59 (inclusive start)" },
        { At(16, 0), 18, "15:30–16:59 (within)" },
        { JustBefore(17, 0), 18, "15:30–16:59 (exclusive end)" },

        { At(17, 0), 13, "17:00–17:59 (inclusive start)" },
        { At(17, 30), 13, "17:00–17:59 (within)" },
        { JustBefore(18, 0), 13, "17:00–17:59 (exclusive end)" },

        { At(18, 0), 8, "18:00–18:29 (inclusive start)" },
        { At(18, 15), 8, "18:00–18:29 (within)" },
        { JustBefore(18, 30), 8, "18:00–18:29 (exclusive end)" },

        { At(18, 30), 0, "18:30–05:59 (inclusive start)" },
        { At(2, 0), 0, "18:30–05:59 (within)" },
        { JustBefore(6, 0), 0, "18:30–05:59 (exclusive end)" }
    };

    [Theory]
    [MemberData(nameof(SampleIntervalTimes))]
    public void GetFee_ReturnsExpectedFee(TimeOnly time, int expectedFee, string _)
    {
        int fee = TollFeeSchedules.Standard.GetFee(time);

        Assert.Equal(expectedFee, fee);
    }
}
