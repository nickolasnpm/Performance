using Microsoft.EntityFrameworkCore;
using Performance.Domain;
using Performance.Domain.Entity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Performance.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<CreditCardStatement> CreditCardStatements { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanRepayment> LoanRepayments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<SupportTicketComment> SupportTicketComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("PerformanceTemp");
        }
    }
}
