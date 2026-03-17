using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.CreditCardStatements
{
    public class CreditCardStatementDTO
    {
        public long Id { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal StatementBalance { get; set; } = decimal.Zero;
        public decimal MinimumPayment { get; set; } = decimal.Zero;
        public decimal PaymentsReceived { get; set; } = decimal.Zero;
        public decimal InterestCharged { get; set; } = decimal.Zero;
        public decimal AvailableCredit { get; set; } = decimal.Zero;
        public long CreditCardId { get; set; }


        // only for mapping from CreditCardStatement entity to CreditCardStatementDTO
        [SetsRequiredMembers]
        public CreditCardStatementDTO(CreditCardStatement statement)
        {
            Id = statement.Id;
            StatementDate = statement.StatementDate;
            DueDate = statement.DueDate;
            StatementBalance = statement.StatementBalance;
            MinimumPayment = statement.MinimumPayment;
            PaymentsReceived = statement.PaymentsReceived;
            InterestCharged = statement.InterestCharged;
            AvailableCredit = statement.AvailableCredit;
            CreditCardId = statement.CreditCardId;
        }
    }
}