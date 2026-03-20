using Performance.Application.DTOs.BankTransactions;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.BankTransactions
{
    public static class BankTransactionMapper
    {
        public static BankTransactionDTO ToDTO(BankTransaction bankTransaction) => new()
        {
            Id = bankTransaction.Id,
            BaseAmount = bankTransaction.BaseAmount,
            FeeAmount = bankTransaction.FeeAmount,
            SettlementAmount = bankTransaction.SettlementAmount,
            TransactionType = bankTransaction.TransactionType,
            Status = bankTransaction.Status,
            MerchantName = bankTransaction.MerchantName,
            ReferenceNumber = bankTransaction.ReferenceNumber,
            BankAccountId = bankTransaction.BankAccountId
        };
    }
}