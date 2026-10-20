using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollExemptionPolicies;

public interface ITollFreeVehiclePolicy
{
    bool IsTollFree(IVehicle vehicle);
}
