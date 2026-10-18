using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator.Vehicles
{
    public class Tractor : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Tractor;
        }
    }
}
