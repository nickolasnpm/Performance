using System.Diagnostics.CodeAnalysis;
using Performance.Application.DTOs.CreditCardStatements;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.CreditCards
{
    public class CreditCardDTO
    {
        public long Id { get; set; }
        public required string CardNumber { get; set; }
        public required string CardHolderName { get; set; }
        public required string CardProvider { get; set; }
        public required string Bank { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public bool IsDefault { get; set; }
        public decimal CreditLimit { get; set; } = decimal.Zero;
        public long UserId { get; set; }
        public List<CreditCardStatementDTO> Statements { get; set; } = new List<CreditCardStatementDTO>();


        // only for mapping from CreditCard entity to CreditCardDTO
        [SetsRequiredMembers]
        public CreditCardDTO(CreditCard creditCard)
        {
            Id = creditCard.Id;
            CardNumber = creditCard.CardNumber;
            CardHolderName = creditCard.CardHolderName;
            CardProvider = creditCard.CardProvider;
            Bank = creditCard.Bank;
            ExpiryMonth = creditCard.ExpiryMonth;
            ExpiryYear = creditCard.ExpiryYear;
            IsDefault = creditCard.IsDefault;
            CreditLimit = creditCard.CreditLimit;
            UserId = creditCard.UserId;
            if (creditCard.Statements != null)
                Statements = creditCard.Statements.Select(s => new CreditCardStatementDTO(s)).ToList();
        }
    }
}