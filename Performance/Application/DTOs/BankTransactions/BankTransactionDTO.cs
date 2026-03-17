using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.BankTransactions
{
    public class BankTransactionDTO
    {
        public long Id { get; set; }
        public decimal BaseAmount { get; set; } = decimal.Zero;
        public decimal FeeAmount { get; set; } = decimal.Zero;
        public decimal SettlementAmount { get; set; } = decimal.Zero;
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public required string MerchantName { get; set; }
        public required string ReferenceNumber { get; set; }
        public long BankAccountId { get; set; }


        // only for mapping from BankTransaction entity to BankTransactionDTO
        [SetsRequiredMembers]
        public BankTransactionDTO(BankTransaction bankTransaction)
        {
            Id = bankTransaction.Id;
            BaseAmount = bankTransaction.BaseAmount;
            FeeAmount = bankTransaction.FeeAmount;
            SettlementAmount = bankTransaction.SettlementAmount;
            TransactionType = bankTransaction.TransactionType;
            Status = bankTransaction.Status;
            MerchantName = bankTransaction.MerchantName;
            ReferenceNumber = bankTransaction.ReferenceNumber;
            BankAccountId = bankTransaction.BankAccountId;
        }
    }
}