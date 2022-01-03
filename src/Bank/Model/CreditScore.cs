namespace Bank.Model;

public record CreditScore
{
    public int Score { get; set; }
    public int History { get; set; }
}