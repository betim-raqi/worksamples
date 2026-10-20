using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public class TollCalculator
{
    private readonly ITollExemptionPolicy _tollExemptionPolicy;
    private readonly TollCalculatorSettings _settings;

    public TollCalculator(ITollExemptionPolicy tollExemptionPolicy, TollCalculatorSettings settings)
    {
        _tollExemptionPolicy = tollExemptionPolicy;
        _settings = settings;
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

        var passes = dates
            .Order()
            .Select(date => new TollPass(date, GetTollFee(date, vehicle)));

        int totalFee = SplitIntoWindows(passes, _settings.IntervalWindow)
            .Sum(window => window.HighestFee);

        return Math.Min(totalFee, _settings.MaxDailyFee);
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
        if (_tollExemptionPolicy.IsTollFree(DateOnly.FromDateTime(date), vehicle)) return 0;

        int fee = GetFee(TimeOnly.FromDateTime(date));

        return fee;
    }

    private int GetFee(TimeOnly time)
    {
        TollFeeInterval? interval = _settings.FeeSchedule.FirstOrDefault(i => time >= i.Start && time < i.End);

        int fee = interval?.Fee ?? 0;

        return fee;
    }
}