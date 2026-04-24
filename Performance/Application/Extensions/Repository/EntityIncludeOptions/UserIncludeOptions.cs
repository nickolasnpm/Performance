using Microsoft.EntityFrameworkCore;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Repository.EntityIncludeOptions
{
    public record UserIncludeOptions: BaseIncludeOptions<User>
    {
        public bool Roles { get; init; }
        public bool Address { get; init; }
        public bool BankAccount { get; init; }
        public bool CreditCards { get; init; }
        public bool Loans { get; init; }
        public bool SupportTickets { get; init; }

        public static UserIncludeOptions All => new()
        {
            Roles = true,
            Address = true,
            BankAccount = true,
            CreditCards = true,
            Loans = true,
            SupportTickets = true
        };

        internal override IQueryable<User> ApplyTo(IQueryable<User> query)
        {
            if (Roles) query = query.Include(u => u.Roles);
            if (Address) query = query.Include(u => u.Address);
            if (BankAccount) query = query.Include(u => u.BankAccount).ThenInclude(ba => ba != null ? ba.Transactions : null);
            if (CreditCards) query = query.Include(u => u.CreditCards).ThenInclude(cc => cc.Statements);
            if (Loans) query = query.Include(u => u.Loans).ThenInclude(l => l.Repayments);
            if (SupportTickets) query = query.Include(u => u.SupportTickets).ThenInclude(st => st.Comments);

            return query;
        }

        public static UserIncludeOptions None => new();
    }
}
