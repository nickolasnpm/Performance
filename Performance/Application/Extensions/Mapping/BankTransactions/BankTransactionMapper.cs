using Performance.Application.DTOs.BankTransactions;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.BankTransactions
{
    public static class BankTransactionMapper
    {
        public static BankTransactionDTO ToDTO(BankTransaction bankTransaction, IIdHelper idHelper) => new(
            Id: idHelper.EncryptId(bankTransaction.Id),
            BaseAmount: bankTransaction.BaseAmount,
            FeeAmount: bankTransaction.FeeAmount,
            SettlementAmount: bankTransaction.SettlementAmount,
            TransactionType: bankTransaction.TransactionType,
            Status: bankTransaction.Status,
            MerchantName: bankTransaction.MerchantName,
            ReferenceNumber: bankTransaction.ReferenceNumber
        );
    }
}