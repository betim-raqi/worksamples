using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Vehicles
{
    public class Car : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Car;
        }
    }
}