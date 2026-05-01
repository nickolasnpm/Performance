using Performance.Application.DTOs.BankAccounts;
using Performance.Application.Extensions.Mapping.BankTransactions;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.BankAccounts
{
    public static class BankAccountMapper
    {
        public static BankAccountDTO ToDTO(BankAccount bankAccount, IIdHelper idHelper) => new(
            Id:  idHelper.EncryptId(bankAccount.Id),
            AccountNumber:  bankAccount.AccountNumber,
            CurrentBalance:  bankAccount.CurrentBalance,
            AvailableBalance:  bankAccount.AvailableBalance,
            Transactions:  bankAccount.Transactions?.Map(t => BankTransactionMapper.ToDTO(t, idHelper)) ?? null
        );
    }
}