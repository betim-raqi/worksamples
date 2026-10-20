using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public sealed class TollExemptionPolicy(
    ITollFreeDatePolicy datePolicy,
    ITollFreeVehiclePolicy vehiclePolicy) : ITollExemptionPolicy
{
    public bool IsTollFree(DateOnly date, IVehicle vehicle) =>
        vehiclePolicy.IsTollFree(vehicle) ||
        datePolicy.IsTollFreeDate(date);
}