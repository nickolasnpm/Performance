namespace Performance.Application.Extensions.Repository.EntityIncludeOptions
{
    public record UserIncludeOptions
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

        public static UserIncludeOptions None => new();
    }
}
