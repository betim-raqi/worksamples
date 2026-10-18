using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Vehicles
{
    public class Foreign : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Foreign;
        }
    }
}
