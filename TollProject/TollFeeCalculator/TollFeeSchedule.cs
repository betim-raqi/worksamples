using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator;

public class TollFeeSchedule : ITollFeeSchedule
{
    private readonly TollFeeInterval[] _intervals;

    public TollFeeSchedule(IEnumerable<TollFeeInterval> intervals)
    {
        _intervals = intervals.ToArray();
    }

    public int GetFee(TimeOnly time)
    {
        TollFeeInterval? interval = _intervals.FirstOrDefault(i => time >= i.Start && time < i.End);

        int fee = interval?.Fee ?? 0;

        return fee;
    }
}
