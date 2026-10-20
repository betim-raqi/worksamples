namespace TollFeeCalculator;

public class TollCalculatorSettings
{
    public int MaxDailyFee { get; set; }
    public TimeSpan IntervalWindow { get; set; }
    public List<TollFeeInterval> FeeSchedule { get; set; } = [];

    public void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(MaxDailyFee);

        if (IntervalWindow <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(IntervalWindow), "IntervalWindow must be greater than zero.");
        }
        if (FeeSchedule == null || FeeSchedule.Count == 0)
        {
            throw new ArgumentException("FeeSchedule must contain at least one interval.", nameof(FeeSchedule));
        }

        ThrowIfOverlappingIntervals();
    }

    private void ThrowIfOverlappingIntervals()
    {
        var intervals = FeeSchedule.OrderBy(interval => interval.Start).ToList();
        for (int i = 0; i < intervals.Count - 1; i++)
        {
            if (intervals[i].End > intervals[i + 1].Start)
            {
                throw new ArgumentException("FeeSchedule intervals must not overlap.");
            }
        }
    }
}
