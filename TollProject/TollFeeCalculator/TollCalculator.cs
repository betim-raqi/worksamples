using System;
using System.Globalization;
using TollFeeCalculator;
using TollFeeCalculator.Vehicles;

public class TollCalculator
{
    private readonly ITollFreeDateProvider _tollFreeDateProvider;
    private readonly ITollFeeSchedule _tollFeeSchedule;
    private readonly ITollFreeVehiclePolicy _tollFreeVehiclePolicy;

    public TollCalculator(ITollFreeDateProvider tollFreeDateProvider, ITollFeeSchedule tollFeeSchedule, ITollFreeVehiclePolicy tollFreeVehiclePolicy)
    {
        _tollFreeDateProvider = tollFreeDateProvider;
        _tollFeeSchedule = tollFeeSchedule;
        _tollFreeVehiclePolicy = tollFreeVehiclePolicy;
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

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (_tollFreeDateProvider.IsTollFreeDate(date) || _tollFreeVehiclePolicy.IsTollFree(vehicle)) return 0;

        TimeOnly time = TimeOnly.FromDateTime(date);

        int fee = _tollFeeSchedule.GetFee(time);

        return fee;
    }
}