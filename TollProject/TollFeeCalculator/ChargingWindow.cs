using System;

namespace TollFeeCalculator;

internal sealed class ChargingWindow
{
    private readonly TimeSpan _length;

    public DateTime Start { get; }
    public int HighestFee { get; private set; }

    public ChargingWindow(DateTime start, TimeSpan length)
    {
        Start = start;
        _length = length;
    }

    public bool Includes(DateTime date) => date - Start <= _length;

    public void AddFee(int fee) => HighestFee = Math.Max(HighestFee, fee);
}