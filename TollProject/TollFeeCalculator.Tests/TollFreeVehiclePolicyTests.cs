using TollFeeCalculator.TollExemptionPolicies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollFreeVehiclePolicyTests
{
    private readonly TollFreeVehiclePolicy _tollFreeVehiclePolicy;
    public TollFreeVehiclePolicyTests()
    {
        _tollFreeVehiclePolicy = new TollFreeVehiclePolicy(TollFreeVehicleSets.Standard);
    }

    [Theory]
    [InlineData(VehicleKind.Motorbike)]
    [InlineData(VehicleKind.Tractor)]
    [InlineData(VehicleKind.Emergency)]
    [InlineData(VehicleKind.Diplomat)]
    [InlineData(VehicleKind.Foreign)]
    [InlineData(VehicleKind.Military)]
    public void IsTollFree_ReturnsTrue_WhenVehicleIsTollFree(VehicleKind vehicleKind)
    {
        bool result = _tollFreeVehiclePolicy.IsTollFree(new Vehicle(vehicleKind));
        
        Assert.True(result);
    }

    [Theory]
    [InlineData(VehicleKind.Car)]
    public void IsTollFree_ReturnsFalse_WhenVehicleIsNotTollFree(VehicleKind vehicleKind)
    {
        bool result = _tollFreeVehiclePolicy.IsTollFree(new Vehicle(vehicleKind));

        Assert.False(result);
    }
}
