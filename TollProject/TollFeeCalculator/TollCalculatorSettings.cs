namespace TollFeeCalculator;

public class TollCalculatorSettings
{
    public int MaxDailyFee { get; set; }
    public TimeSpan IntervalWindow { get; set; }
    public List<TollFeeInterval> FeeSchedule { get; set; } = [];
}
