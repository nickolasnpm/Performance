using Performance.Application.DTOs.CreditCards;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class CreditCardMapper
    {
        extension(CreditCard creditCard)
        {
            public CreditCardDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(creditCard.Id),
                CardNumber: creditCard.CardNumber,
                CardHolderName: creditCard.CardHolderName,
                CardProvider: creditCard.CardProvider,
                Bank: creditCard.Bank,
                ExpiryMonth: creditCard.ExpiryMonth,
                ExpiryYear: creditCard.ExpiryYear,
                IsDefault: creditCard.IsDefault,
                CreditLimit: creditCard.CreditLimit,
                Statements: creditCard.Statements?.Select(cc => cc.ToDTO(idHelper)).ToList()
            );
        }
    }
}