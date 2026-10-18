using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public class TollFreeVehiclePolicy : ITollFreeVehiclePolicy
    {
        private static readonly HashSet<VehicleKind> TollFreeVehicles =
        [
            VehicleKind.Motorbike,
            VehicleKind.Tractor,
            VehicleKind.Emergency,
            VehicleKind.Diplomat,
            VehicleKind.Foreign,
            VehicleKind.Military
        ];

        public bool IsTollFree(IVehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            VehicleKind vehicleKind = vehicle.GetVehicleType();

            bool isTollFree = TollFreeVehicles.Contains(vehicleKind);

            return isTollFree;
        }
    }
}
