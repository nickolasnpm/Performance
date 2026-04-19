using Performance.Application.Common.Prefix;
using Performance.Application.DTOs.BankTransactions;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.BankTransactions
{
    public static class BankTransactionMapper
    {
        public static BankTransactionDTO ToDTO(BankTransaction bankTransaction, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncodeId(bankTransaction.Id, IdPrefix.BankTransaction),
            BaseAmount = bankTransaction.BaseAmount,
            FeeAmount = bankTransaction.FeeAmount,
            SettlementAmount = bankTransaction.SettlementAmount,
            TransactionType = bankTransaction.TransactionType,
            Status = bankTransaction.Status,
            MerchantName = bankTransaction.MerchantName,
            ReferenceNumber = bankTransaction.ReferenceNumber,
            BankAccountId = idHelper.EncodeId(bankTransaction.BankAccountId, IdPrefix.BankAccount)
        };
    }
}