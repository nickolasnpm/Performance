using Performance.Application.DTOs.CreditCardStatements;

namespace Performance.Application.DTOs.CreditCards
{
    public record CreditCardDTO(
        string Id,
        string CardNumber,
        string CardHolderName,
        string CardProvider,
        string Bank,
        int ExpiryMonth,
        int ExpiryYear,
        bool IsDefault,
        decimal CreditLimit,
        List<CreditCardStatementDTO>? Statements);
}