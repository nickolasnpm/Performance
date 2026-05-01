using Performance.Application.DTOs.BankTransactions;

namespace Performance.Application.DTOs.BankAccounts
{
    public record BankAccountDTO(
        string Id,
        string AccountNumber,
        decimal CurrentBalance,
        decimal AvailableBalance,
        List<BankTransactionDTO>? Transactions);
}
