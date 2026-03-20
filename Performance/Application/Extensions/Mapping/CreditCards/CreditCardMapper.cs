using Performance.Application.DTOs.CreditCards;
using Performance.Application.Extensions.Mapping.CreditCardStatements;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.CreditCards
{
    public static class CreditCardMapper
    {
        public static CreditCardDTO ToDTO(CreditCard creditCard) => new()
        {
            Id = creditCard.Id,
            CardNumber = creditCard.CardNumber,
            CardHolderName = creditCard.CardHolderName,
            CardProvider = creditCard.CardProvider,
            Bank = creditCard.Bank,
            ExpiryMonth = creditCard.ExpiryMonth,
            ExpiryYear = creditCard.ExpiryYear,
            IsDefault = creditCard.IsDefault,
            CreditLimit = creditCard.CreditLimit,
            UserId = creditCard.UserId,
            Statements = creditCard.Statements != null ? creditCard.Statements.MapToDTO(CreditCardStatementMapper.ToDTO) : null
        };
    }
}