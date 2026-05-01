namespace Performance.Application.DTOs.CreditCardStatements
{
    public record CreditCardStatementDTO(
        string Id,
        DateTime StatementDate,
        DateTime DueDate,
        decimal StatementBalance,
        decimal MinimumPayment,
        decimal PaymentsReceived,
        decimal InterestCharged,
        decimal AvailableCredit);
}