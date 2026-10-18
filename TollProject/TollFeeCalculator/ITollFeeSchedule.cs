using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public interface ITollFeeSchedule
    {
        int GetFee(int hour, int minute);
    }
}
