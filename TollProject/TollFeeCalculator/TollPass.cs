using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator;

public class TollPass
{
    public TollPass(DateTime time, int fee)
    {
        Time = time;
        Fee = fee;
    }
    public DateTime Time { get; set; }
    public int Fee { get; set; }
}
