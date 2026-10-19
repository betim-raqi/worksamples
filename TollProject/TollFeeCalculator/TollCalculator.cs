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

            TimeSpan intervalWindow = TimeSpan.FromMinutes(IntervalWindowInMinutes);
            var passes = dates
                .Order()
                .Select(date => new TollPass(date, GetTollFee(date, vehicle)));

            int totalFee = SplitIntoWindows(passes, intervalWindow)
                .Sum(window => window.HighestFee);

            return Math.Min(totalFee, MaxTollFeePerDay);
        }

        private static List<ChargingWindow> SplitIntoWindows(IEnumerable<TollPass> tollPasses, TimeSpan windowLength)
        {
            List<ChargingWindow> windows = [];
            ChargingWindow? currentWindow = null;

            foreach (TollPass pass in tollPasses)
            {
                if (currentWindow is null || !currentWindow.Includes(pass.Time))
                {
                    currentWindow = new ChargingWindow(pass.Time, windowLength);
                    windows.Add(currentWindow);
                }

                currentWindow.AddFee(pass.Fee);
            }

            return windows;
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