using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator.Vehicles
{
    public class Motorbike : IVehicle
    {
        public VehicleKind GetVehicleType()
        {
            return VehicleKind.Motorbike;
        }
    }
}
