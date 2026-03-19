using Performance.Application.DTOs.BankTransactions;

namespace Performance.Application.DTOs.BankAccounts
{
    public class BankAccountDTO
    {
        public long Id { get; set; }
        public required string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public long UserId { get; set; }
        public List<BankTransactionDTO>? Transactions { get; set; }
    }
}