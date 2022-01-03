using Bank.Model;
using Microsoft.Extensions.Options;

namespace Bank;

public class QuotationProvider
{
    private readonly BankConfiguration _config;

    public QuotationProvider(IOptions<BankConfiguration> config)
    {
        this._config = config.Value;
    }

    public bool IsEligibleForLoan(QuotationRequest request)
    {
        return request.Amount <= _config.MaxLoanAmount && request.CreditScore.Score >= _config.MinimumCreditScore;

    }

    public Quotation ProvideQuotation(QuotationRequest request)
    {
        var rate = CalculateRate(request.CreditScore.Score);

        return new Quotation("Accepted", rate, _config.BankId, request.RequestId);
    }
    
    private double CalculateRate(int creditScore)
    {
        var random = new Random();
        return _config.BaseRate * random.Next(10) /10 * ((1000 - creditScore) / 100f);
    }
}