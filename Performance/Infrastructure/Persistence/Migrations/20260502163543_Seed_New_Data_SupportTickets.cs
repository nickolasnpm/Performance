using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_SupportTickets : Migration
    {
        private const int totalData = 1000000;
        private const int startingNumber = 110000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i      INT = {startingNumber};
                DECLARE @userId BIGINT;

                WHILE @i < {endingNumber}
                BEGIN
                    SELECT @userId = [Id]
                    FROM [Performance].[Users]
                    WHERE [Username] = N'user' + CAST(@i AS NVARCHAR(10));

                    INSERT INTO [Performance].[SupportTickets]
                        ([Subject],
                         [Description],
                         [Priority],
                         [IsResolved],
                         [UserId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            -- Subject: cycles between 5 subjects
                            CASE (@i % 5)
                                WHEN 0 THEN N'Loan repayment query'
                                WHEN 1 THEN N'Update contact information'
                                WHEN 2 THEN N'Transaction not reflected in statement'
                                WHEN 3 THEN N'Unable to access my account'
                                WHEN 4 THEN N'Credit card payment issue'
                            END,

                            -- Description: cycles between 5 descriptions with reference number
                            CASE (@i % 5)
                                WHEN 0 THEN N'I need clarification on my loan repayment schedule and interest calculation.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(@i AS NVARCHAR(14)), 14)
                                WHEN 1 THEN N'I would like to update my phone number and email address on file.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(@i AS NVARCHAR(14)), 14)
                                WHEN 2 THEN N'My recent transaction is not showing in my bank statement. Transaction reference: REF' + RIGHT(REPLICATE(N'0', 14) + CAST(@i AS NVARCHAR(14)), 14)
                                WHEN 3 THEN N'I am unable to login to my account. Please help me reset my password.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(@i AS NVARCHAR(14)), 14)
                                WHEN 4 THEN N'I made a credit card payment but it has not been processed yet.REF' + RIGHT(REPLICATE(N'0', 14) + CAST(@i AS NVARCHAR(14)), 14)
                            END,

                            -- Priority: cycles 1 / 2 / 3
                            (@i % 3) + 1,

                            -- IsResolved: always 0
                            CAST(0 AS BIT),

                            @userId,

                            SYSDATETIMEOFFSET(),
                            N'data seeding',
                            SYSDATETIMEOFFSET(),
                            N'data seeding'
                        );

                    SET @i = @i + 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Performance].[SupportTickets]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}