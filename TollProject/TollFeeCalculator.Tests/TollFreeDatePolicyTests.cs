using TollFeeCalculator.TollExemptionPolicies;

namespace TollFeeCalculator.Tests;

public class TollFreeDatePolicyTests
{
    private readonly SwedishTollFreeDatePolicy _tollFreeDatePolicy;
    public TollFreeDatePolicyTests()
    {
        _tollFreeDatePolicy = new SwedishTollFreeDatePolicy();
    }

    [Theory]
    [MemberData(nameof(PublicHolidayDates))]
    public void IsTollFreeDate_ReturnsTrue_WhenPublicHoliday(DateOnly date)
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(date);

        Assert.True(isTollFree);
    }

    [Theory]
    [MemberData(nameof(PublicHolidayDates))]
    public void IsTollFreeDate_ReturnsTrue_WhenDayBeforePublicHoliday(DateOnly date)
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(date.AddDays(-1));
        
        Assert.True(isTollFree);
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_WhenSaturday()
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(new(2026, 10, 24));
        Assert.True(isTollFree);
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_WhenSunday()
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(new(2026, 10, 25));
        Assert.True(isTollFree);
    }

    [Fact]
    public void IsTollFreeDate_ReturnsTrue_WhenJuly()
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(new(2026, 7, 15));
        Assert.True(isTollFree);
    }

    [Fact]
    public void IsTollFreeDate_ReturnsFalse_WhenRegularWeekday()
    {
        bool isTollFree = _tollFreeDatePolicy.IsTollFree(new(2026, 10, 20));
        Assert.False(isTollFree);
    }

    public static TheoryData<DateOnly> PublicHolidayDates => new()
    {
        { new DateOnly(2013, 1, 1) },
        { new DateOnly(2013, 3, 29) },
        { new DateOnly(2013, 4, 1) },
        { new DateOnly(2013, 5, 1) },
        { new DateOnly(2013, 5, 9) },
        { new DateOnly(2013, 6, 6) },
        { new DateOnly(2013, 6, 22) },
        { new DateOnly(2013, 11, 2) },
        { new DateOnly(2013, 12, 25) },
        { new DateOnly(2013, 12, 26) },

        { new DateOnly(2014, 1, 1) },

        { new DateOnly(2026, 1, 1) },
        { new DateOnly(2026, 1, 6) },
        { new DateOnly(2026, 4, 3) },
        { new DateOnly(2026, 4, 6) },
        { new DateOnly(2026, 5, 1) },
        { new DateOnly(2026, 5, 14) },
        { new DateOnly(2026, 6, 6) },
        { new DateOnly(2026, 6, 20) },
        { new DateOnly(2026, 10, 31) },
        { new DateOnly(2026, 12, 25) },

        { new DateOnly(2027, 1, 1) }
    };
}
