namespace TollFeeCalculator.Vehicles;

public class Vehicle(VehicleKind vehicleKind) : IVehicle
{
    public VehicleKind GetVehicleType() => vehicleKind;
}
