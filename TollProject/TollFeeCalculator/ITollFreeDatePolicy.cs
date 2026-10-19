namespace TollFeeCalculator;

public interface ITollFreeDatePolicy
{
    bool IsTollFreeDate(DateOnly date);
}
