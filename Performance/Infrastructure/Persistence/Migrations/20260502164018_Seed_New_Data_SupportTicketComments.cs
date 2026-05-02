using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_SupportTicketComments : Migration
    {
        private const int totalData = 1_000_000;
        private const int startingNumber = 110_000;
        private readonly int endingNumber = totalData + startingNumber;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                DECLARE @i        INT = {startingNumber};
                DECLARE @ticketId BIGINT;

                WHILE @i < {endingNumber}
                BEGIN
                    SELECT @ticketId = [Id]
                    FROM [Performance].[SupportTickets]
                    WHERE [UserId] = (
                        SELECT [Id]
                        FROM [Performance].[Users]
                        WHERE [Username] = N'user' + CAST(@i AS NVARCHAR(10))
                    );

                    INSERT INTO [Performance].[SupportTicketComments]
                        ([CommentText],
                         [RecommendedAction],
                         [TicketId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            -- CommentText: cycles between 5
                            CASE (@i % 5)
                                WHEN 0 THEN N'Thank you for contacting support. We are looking into your issue.'
                                WHEN 1 THEN N'Our technical team is investigating this matter.'
                                WHEN 2 THEN N'We appreciate your patience while we resolve this issue.'
                                WHEN 3 THEN N'Your request has been escalated to our specialist team.'
                                WHEN 4 THEN N'We have reviewed your account and are working on a resolution.'
                            END,

                            -- RecommendedAction: cycles between 5
                            CASE (@i % 5)
                                WHEN 0 THEN N'Please verify your account details and try again.'
                                WHEN 1 THEN N'Try clearing your browser cache and logging in again.'
                                WHEN 2 THEN N'Please review the attached documentation for more information.'
                                WHEN 3 THEN N'Contact us at support@bank.com if the issue persists.'
                                WHEN 4 THEN N'We recommend waiting 24-48 hours for the transaction to reflect.'
                            END,

                            @ticketId,

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
                DELETE FROM [Performance].[SupportTicketComments]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}