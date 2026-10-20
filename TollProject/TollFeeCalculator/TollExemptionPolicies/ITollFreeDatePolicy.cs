namespace TollFeeCalculator.TollExemptionPolicies;

public interface ITollFreeDatePolicy
{
    bool IsTollFree(DateOnly date);
}
