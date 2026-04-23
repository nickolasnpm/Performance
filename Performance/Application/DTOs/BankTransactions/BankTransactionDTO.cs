using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.BankTransactions
{
    public class BankTransactionDTO
    {
        public required string Id { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal SettlementAmount { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public required string MerchantName { get; set; }
        public required string ReferenceNumber { get; set; }
    }
}