using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public interface ITollFreeVehiclePolicy
{
    bool IsTollFree(IVehicle vehicle);
}
