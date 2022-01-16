namespace Bank;

public record BankConfiguration
{
    public string BankId { get; set; } = null!;
    public decimal MaxLoanAmount { get; set; }
    public int MinimumCreditScore { get; set; }
    public double BaseRate { get; set; }
}