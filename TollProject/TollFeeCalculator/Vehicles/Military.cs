using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Vehicles
{
    public class Military : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Military;
        }
    }
}
