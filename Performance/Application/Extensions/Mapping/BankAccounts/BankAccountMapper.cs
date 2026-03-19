using Performance.Application.DTOs.BankAccounts;
using Performance.Application.Extensions.Mapping.BankTransactions;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.BankAccounts
{
    public static class BankAccountMapper
    {
        public static BankAccountDTO ToDTO(this BankAccount bankAccount) => new()
        {
            Id = bankAccount.Id,
            AccountNumber = bankAccount.AccountNumber,
            CurrentBalance = bankAccount.CurrentBalance,
            AvailableBalance = bankAccount.AvailableBalance,
            UserId = bankAccount.UserId,
            Transactions = bankAccount.Transactions != null ? bankAccount.Transactions.Select(t => t.ToDTO()).ToList() : null
        };
    }
}