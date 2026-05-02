using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Performance.Migrations
{
    /// <inheritdoc />
    public partial class Seed_New_Data_CreditCards : Migration
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

                    INSERT INTO [Performance].[CreditCards]
                        ([CardNumber],
                         [CardHolderName],
                         [CardProvider],
                         [Bank],
                         [ExpiryMonth],
                         [ExpiryYear],
                         [IsDefault],
                         [CreditLimit],
                         [UserId],
                         [CreatedAt],
                         [CreatedBy],
                         [UpdatedAt],
                         [UpdatedBy])
                    VALUES
                        (
                            -- CardNumber: 4532 prefix + 12 digit zero-padded @i
                            N'4532' + RIGHT(REPLICATE(N'0', 12) + CAST(@i AS NVARCHAR(12)), 12),

                            -- CardHolderName
                            N'First' + CAST(@i AS NVARCHAR(10)) + N' Last' + CAST(@i AS NVARCHAR(10)),

                            -- CardProvider: cycles Visa / Mastercard
                            CASE (@i % 2)
                                WHEN 0 THEN N'Visa'
                                WHEN 1 THEN N'Mastercard'
                            END,

                            -- Bank: cycles BSN / Maybank
                            CASE (@i % 2)
                                WHEN 0 THEN N'BSN'
                                WHEN 1 THEN N'Maybank'
                            END,

                            -- ExpiryMonth: always 12
                            12,

                            -- ExpiryYear: always 2031
                            2031,

                            -- IsDefault: always 1
                            CAST(1 AS BIT),

                            -- CreditLimit: always 10000
                            CAST(10000 AS DECIMAL(18, 2)),

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
                DELETE FROM [Performance].[CreditCards]
                WHERE [CreatedBy] = 'data seeding';
            ");
        }
    }
}