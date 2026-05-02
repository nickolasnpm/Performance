using Performance.Application.DTOs.BankAccounts;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class BankAccountMapper
    {
        extension(BankAccount bankAccount)
        {
            public BankAccountDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(bankAccount.Id),
                AccountNumber: bankAccount.AccountNumber,
                CurrentBalance: bankAccount.CurrentBalance,
                AvailableBalance: bankAccount.AvailableBalance,
                Transactions: bankAccount.Transactions?.Select(t => t.ToDTO(idHelper)).ToList()
            );
        }
    }
}