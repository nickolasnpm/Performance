using Performance.Application.DTOs.BankTransactions;

namespace Performance.Application.DTOs.BankAccounts
{
    public class BankAccountDTO
    {
        public required string Id { get; set; }
        public required string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public required string UserId { get; set; }
        public List<BankTransactionDTO>? Transactions { get; set; }
    }
}