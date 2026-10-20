using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public static class TollFreeVehicleSets
{
    public static readonly HashSet<VehicleKind> Standard =
    [
        VehicleKind.Motorbike,
        VehicleKind.Tractor,
        VehicleKind.Emergency,
        VehicleKind.Diplomat,
        VehicleKind.Foreign,
        VehicleKind.Military
    ];
}
