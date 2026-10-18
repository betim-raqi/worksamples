using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Vehicles
{
    public class Diplomat : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Diplomat;
        }
    }
}
