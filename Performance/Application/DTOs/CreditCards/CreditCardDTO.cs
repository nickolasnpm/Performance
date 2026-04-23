using Performance.Application.DTOs.CreditCardStatements;

namespace Performance.Application.DTOs.CreditCards
{
    public class CreditCardDTO
    {
        public required string Id { get; set; }
        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public required string CardProvider { get; set; }
        public required string Bank { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public bool IsDefault { get; set; }
        public decimal CreditLimit { get; set; }
        public List<CreditCardStatementDTO>? Statements { get; set; }
    }
}