using Performance.Application.DTOs.CreditCardStatements;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.CreditCardStatements
{
    public static class CreditCardStatementMapper
    {
        public static CreditCardStatementDTO ToDTO(this CreditCardStatement statement) => new()
        {
            Id = statement.Id,
            StatementDate = statement.StatementDate,
            DueDate = statement.DueDate,
            StatementBalance = statement.StatementBalance,
            MinimumPayment = statement.MinimumPayment,
            PaymentsReceived = statement.PaymentsReceived,
            InterestCharged = statement.InterestCharged,
            AvailableCredit = statement.AvailableCredit,
            CreditCardId = statement.CreditCardId
        };
    }
}