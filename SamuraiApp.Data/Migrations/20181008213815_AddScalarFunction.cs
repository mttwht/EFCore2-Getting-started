using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class AddScalarFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION [Dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
                RETURNS char(30) AS
                BEGIN
                    DECLARE @ret char(30);
                    SELECT TOP 1 @ret = Name
                    FROM Battles
                    WHERE Battles.Id IN (
                        SELECT BattleId
                        FROM SamuraiBattle
                        WHERE SamuraiId = @samuraiId
                    )
                    ORDER BY StartDate
                    RETURN @ret;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION [Dbo].[EarliestBattleFoughtBySamurai]");
        }
    }
}
