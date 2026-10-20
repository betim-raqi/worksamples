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
    }
}
