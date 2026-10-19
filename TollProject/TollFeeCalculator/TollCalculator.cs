using System;
using System.Globalization;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        private const int MaxTollFeePerDay = 60;
        private const int IntervalWindowInMinutes = 60;
        private readonly ITollFreeDateProvider _tollFreeDateProvider;
        private readonly ITollFeeSchedule _tollFeeSchedule;
        private readonly ITollFreeVehiclePolicy _tollFreeVehiclePolicy;

        public TollCalculator(
            ITollFreeDateProvider tollFreeDateProvider,
            ITollFeeSchedule tollFeeSchedule,
            ITollFreeVehiclePolicy tollFreeVehiclePolicy)
        {
            _tollFreeDateProvider = tollFreeDateProvider;
            _tollFeeSchedule = tollFeeSchedule;
            _tollFreeVehiclePolicy = tollFreeVehiclePolicy;
        }

        private static void ThrowIfDatesOnDifferentDays(DateTime[] dates, string paramName)
        {
            if (dates.Length > 0 && dates.Any(d => d.Date != dates[0].Date))
            {
                throw new ArgumentException("All dates must be on the same day.", paramName);
            }
        }

        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <param name="vehicle">The vehicle</param>
        /// <param name="dates">Date and time of all passes on one day</param>
        /// <returns>The total toll fee for that day</returns>
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));
            ArgumentNullException.ThrowIfNull(dates, nameof(dates));
            ThrowIfDatesOnDifferentDays(dates, nameof(dates));

            if (dates.Length == 0) { return 0; }
            DateTime[] sortedDates = dates.Order().ToArray();

            DateTime intervalStart = sortedDates[0];
            TimeSpan intervalWindow = TimeSpan.FromMinutes(IntervalWindowInMinutes);
            int totalFee = 0;
            int intervalFee = 0;
            foreach (DateTime date in sortedDates)
            {
                int nextFee = GetTollFee(date, vehicle);
                TimeSpan diff = date - intervalStart;

                if (diff <= intervalWindow)
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
            if (totalFee > MaxTollFeePerDay) totalFee = MaxTollFeePerDay;
            return totalFee;
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle, nameof(vehicle));

            if (_tollFreeDateProvider.IsTollFreeDate(date) || _tollFreeVehiclePolicy.IsTollFree(vehicle)) return 0;

            TimeOnly time = TimeOnly.FromDateTime(date);

            int fee = _tollFeeSchedule.GetFee(time);

            return fee;
        }
    }
}