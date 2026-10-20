using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public interface ITollExemptionPolicy
{
    bool IsTollFree(DateOnly date, IVehicle vehicle);
}
