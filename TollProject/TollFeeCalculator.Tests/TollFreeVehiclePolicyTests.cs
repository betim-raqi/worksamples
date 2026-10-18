using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests
{
    public class TollFreeVehiclePolicyTests
    {
        [Theory]
        [InlineData(VehicleKind.Motorbike)]
        [InlineData(VehicleKind.Tractor)]
        [InlineData(VehicleKind.Emergency)]
        [InlineData(VehicleKind.Diplomat)]
        [InlineData(VehicleKind.Foreign)]
        [InlineData(VehicleKind.Military)]
        public void IsTollFree_ReturnsTrue_WhenVehicleIsTollFree(VehicleKind vehicleKind)
        {
            TollFreeVehiclePolicy policy = new TollFreeVehiclePolicy();
            IVehicle vehicle = new Vehicle(vehicleKind);
            bool result = policy.IsTollFree(vehicle);
            
            Assert.True(result);
        }

        [Theory]
        [InlineData(VehicleKind.Car)]
        public void IsTollFree_ReturnsFalse_WhenVehicleIsNotTollFree(VehicleKind vehicleKind)
        {
            TollFreeVehiclePolicy policy = new TollFreeVehiclePolicy();
            IVehicle vehicle = new Vehicle(vehicleKind);
            bool result = policy.IsTollFree(vehicle);

            Assert.False(result);
        }
    }
}
