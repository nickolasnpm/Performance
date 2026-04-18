using Performance.Application.DTOs.CreditCards;
using Performance.Application.Extensions.Mapping.CreditCardStatements;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.CreditCards
{
    public static class CreditCardMapper
    {
        public static CreditCardDTO ToDTO(CreditCard creditCard, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncodeId(creditCard.Id),
            CardNumber = creditCard.CardNumber,
            CardHolderName = creditCard.CardHolderName,
            CardProvider = creditCard.CardProvider,
            Bank = creditCard.Bank,
            ExpiryMonth = creditCard.ExpiryMonth,
            ExpiryYear = creditCard.ExpiryYear,
            IsDefault = creditCard.IsDefault,
            CreditLimit = creditCard.CreditLimit,
            UserId = idHelper.EncodeId(creditCard.UserId),
            Statements = creditCard.Statements?.Map(cc => CreditCardStatementMapper.ToDTO(cc, idHelper)).ToList() ?? null
        };
    }
}