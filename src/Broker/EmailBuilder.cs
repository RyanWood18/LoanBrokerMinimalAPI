using System.Text;

namespace Broker
{
    public static class EmailBuilder
    {
        private static string BuildQuotesTable(List<Quotation> quotes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<table>");
            builder.AppendLine("<thead><tr><th>Bank</th><th>Loan Rate</th><thead>");
            builder.AppendLine("<tbody>");
            foreach (var quote in quotes)
            {
                builder.AppendLine($"<tr><td>{quote.BankId}</td><td>{quote.Rate:N2}</td></tr>");
            }
            builder.AppendLine("</tbody>");
            builder.AppendLine("</table>");

            return builder.ToString();
        }

        public static string BuildEmail(List<Quotation> quotes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("<html><head></head>");
            builder.AppendLine("<body>");
            if (quotes.Count > 0)
            {
                builder.AppendLine("<p>Here are your quotes</p>");
                builder.AppendLine(BuildQuotesTable(quotes));
            }
            else
            {
                builder.AppendLine(
                    "<p>Unfortunately no banks could fulfil your loan request. Try adjusting the amount you need to borrow");
            }

            builder.AppendLine("</body>");
            return builder.ToString();
        }
    }
}