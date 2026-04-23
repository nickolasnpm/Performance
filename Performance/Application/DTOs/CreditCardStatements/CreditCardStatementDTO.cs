namespace Performance.Application.DTOs.CreditCardStatements
{
    public class CreditCardStatementDTO
    {
        public required string Id { get; set; }
        public DateTime StatementDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal StatementBalance { get; set; }
        public decimal MinimumPayment { get; set; }
        public decimal PaymentsReceived { get; set; }
        public decimal InterestCharged { get; set; }
        public decimal AvailableCredit { get; set; }
    }
}