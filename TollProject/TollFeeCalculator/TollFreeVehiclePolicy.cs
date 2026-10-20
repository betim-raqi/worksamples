using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public class TollFreeVehiclePolicy : ITollFreeVehiclePolicy
{
    private readonly HashSet<VehicleKind> _tollFreeVehicles;

    public TollFreeVehiclePolicy(IEnumerable<VehicleKind> tollFreeVehicles)
    {
        ArgumentNullException.ThrowIfNull(tollFreeVehicles);

        _tollFreeVehicles = tollFreeVehicles.ToHashSet();
    }

    public bool IsTollFree(IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        VehicleKind vehicleKind = vehicle.GetVehicleType();

        bool isTollFree = _tollFreeVehicles.Contains(vehicleKind);

        return isTollFree;
    }
}
