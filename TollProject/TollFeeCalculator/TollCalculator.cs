using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{
    private readonly ITollFreeDateProvider _tollFreeDateProvider;
    private readonly ITollFeeSchedule _tollFeeSchedule;

    public TollCalculator(ITollFreeDateProvider tollFreeDateProvider, ITollFeeSchedule tollFeeSchedule)
    {
        _tollFreeDateProvider = tollFreeDateProvider;
        _tollFeeSchedule = tollFeeSchedule;
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
        ArgumentNullException.ThrowIfNull(dates, nameof(dates));

        if (dates.Length == 0) { return 0; }
        DateTime[] sortedDates = dates.Order().ToArray();

        DateTime intervalStart = sortedDates[0];
        int totalFee = 0;
        int intervalFee = 0;
        foreach (DateTime date in sortedDates)
        {
            int nextFee = GetTollFee(date, vehicle);

            long diffInMillies = (long)(date - intervalStart).TotalMilliseconds;
            long minutes = diffInMillies/1000/60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= intervalFee;
                if (nextFee >= intervalFee) intervalFee = nextFee;
                totalFee += intervalFee;
            }
            else
            {
                totalFee += nextFee;
                intervalStart = date;
                intervalFee = nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (_tollFreeDateProvider.IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        TimeOnly time = TimeOnly.FromDateTime(date);

        int fee = _tollFeeSchedule.GetFee(time);

        return fee;
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}