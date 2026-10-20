namespace TollFeeCalculator;

public sealed class TollFeeInterval
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }
    public int Fee { get; }

    public TollFeeInterval(TimeOnly start, TimeOnly end, int fee)
    {
        if(start >= end)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }
        ArgumentOutOfRangeException.ThrowIfNegative(fee, nameof(fee));

        Start = start;
        End = end;
        Fee = fee;
    }
}