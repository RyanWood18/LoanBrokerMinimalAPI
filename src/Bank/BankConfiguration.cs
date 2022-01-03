namespace Bank;

public record BankConfiguration
{
    public BankConfiguration(string bankId, decimal maxLoanAmount, int minimumCreditScore, double baseRate)
    {
        BankId = bankId;
        MaxLoanAmount = maxLoanAmount;
        MinimumCreditScore = minimumCreditScore;
        BaseRate = baseRate;
    }

    public string BankId { get; set; }
    public decimal MaxLoanAmount { get; set; }
    public int MinimumCreditScore { get; set; }
    public double BaseRate { get; set; }
}