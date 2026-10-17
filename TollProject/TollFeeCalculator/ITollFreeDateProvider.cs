using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public interface ITollFreeDateProvider
    {
        bool IsTollFreeDate(DateTime date);
    }
}
