using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public class Vehicle(VehicleKind vehicleKind) : IVehicle
    {
        public VehicleKind GetVehicleType() => vehicleKind;
    }
}
