using Microsoft.EntityFrameworkCore;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Repository.EntityIncludeOptions
{
    public static class UserIncludeExtensions
    {
        extension(IQueryable<User> query)
        {
            public IQueryable<User> ApplyIncludes(UserIncludeOptions options)
            {
                if (options.Roles) query = query.Include(u => u.Roles);
                if (options.Address) query = query.Include(u => u.Address);
                if (options.BankAccount) query = query.Include(u => u.BankAccount).ThenInclude(ba => ba != null ? ba.Transactions : null);
                if (options.CreditCards) query = query.Include(u => u.CreditCards).ThenInclude(cc => cc.Statements);
                if (options.Loans) query = query.Include(u => u.Loans).ThenInclude(l => l.Repayments);
                if (options.SupportTickets) query = query.Include(u => u.SupportTickets).ThenInclude(st => st.Comments);

                return query;
            }
        }
    }
}