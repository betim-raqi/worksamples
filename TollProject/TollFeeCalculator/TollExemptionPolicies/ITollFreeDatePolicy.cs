namespace TollFeeCalculator.TollExemptionPolicies;

public interface ITollFreeDatePolicy
{
    bool IsTollFreeDate(DateOnly date);
}
