using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public class StandardTollFeeSchedule : ITollFeeSchedule
    {
        private static readonly TollFeeInterval[] FeeIntervals =
        [
            new(TimeOnly.MinValue, new(6, 0),   0),
            new(new(6, 0),   new(6, 30),   8),
            new(new(6, 30),  new(7, 0),   13),
            new(new(7, 0),   new(8, 0),   18),
            new(new(8, 0),   new(8, 30),  13),
            new(new(8, 30),  new(15, 0),   8),
            new(new(15, 0),  new(15, 30), 13),
            new(new(15, 30), new(17, 0),  18),
            new(new(17, 0),  new(18, 0),  13),
            new(new(18, 0),  new(18, 30),  8),

            // This is an exclusive end, which differs from the other intervals
            // that are inclusive of the end time. Maybe I can find a better way
            // to represent this if time permits...
            new(new(18, 30), TimeOnly.MaxValue, 0),
        ];

        public int GetFee(TimeOnly time)
        {
            TollFeeInterval interval = FeeIntervals.First(i => time >= i.Start && (time < i.End || i.End == TimeOnly.MaxValue));

            return interval.Fee;
        }
    }
}
