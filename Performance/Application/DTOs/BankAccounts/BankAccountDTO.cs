using System.Diagnostics.CodeAnalysis;
using Performance.Application.DTOs.BankTransactions;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.BankAccounts
{
    public class BankAccountDTO
    {
        public long Id { get; set; }
        public required string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; } = decimal.Zero;
        public decimal AvailableBalance { get; set; } = decimal.Zero;
        public long UserId { get; set; }
        public List<BankTransactionDTO> Transactions { get; set; } = new List<BankTransactionDTO>();


        // only for mapping from BankAccount entity to BankAccountDTO
        [SetsRequiredMembers]
        public BankAccountDTO(BankAccount bankAccount)
        {
            Id = bankAccount.Id;
            AccountNumber = bankAccount.AccountNumber;
            CurrentBalance = bankAccount.CurrentBalance;
            AvailableBalance = bankAccount.AvailableBalance;
            UserId = bankAccount.UserId;
            if (bankAccount.Transactions != null)
                Transactions = bankAccount.Transactions.Select(t => new BankTransactionDTO(t)).ToList();
        }
    }
}