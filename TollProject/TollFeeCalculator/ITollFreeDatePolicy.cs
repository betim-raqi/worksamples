using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator;

public interface ITollFreeDatePolicy
{
    bool IsTollFreeDate(DateOnly date);
}
