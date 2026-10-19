namespace TollFeeCalculator;

public sealed class TollFeeInterval
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }
    public int Fee { get; }

    public TollFeeInterval(TimeOnly start, TimeOnly end, int fee)
    {
        Start = start;
        End = end;
        Fee = fee;
    }
}