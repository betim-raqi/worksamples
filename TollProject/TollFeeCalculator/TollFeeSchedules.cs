using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator;

public static class TollFeeSchedules
{
    public static readonly ITollFeeSchedule Standard = new TollFeeSchedule(
    [
        new(new(6, 0),   new(6, 30),   8),
        new(new(6, 30),  new(7, 0),   13),
        new(new(7, 0),   new(8, 0),   18),
        new(new(8, 0),   new(8, 30),  13),
        new(new(8, 30),  new(15, 0),   8),
        new(new(15, 0),  new(15, 30), 13),
        new(new(15, 30), new(17, 0),  18),
        new(new(17, 0),  new(18, 0),  13),
        new(new(18, 0),  new(18, 30),  8)
    ]);
}
