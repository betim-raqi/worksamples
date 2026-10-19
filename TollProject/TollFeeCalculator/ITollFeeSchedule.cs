namespace TollFeeCalculator;

public interface ITollFeeSchedule
{
    int GetFee(TimeOnly time);
}
