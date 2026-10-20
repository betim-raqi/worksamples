using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.TollExemptionPolicies;

public interface ITollExemptionPolicy
{
    bool IsTollFree(DateOnly date, IVehicle vehicle);
}
