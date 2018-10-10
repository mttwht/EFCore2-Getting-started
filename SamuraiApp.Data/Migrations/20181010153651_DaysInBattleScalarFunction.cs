using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class DaysInBattleScalarFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION [Dbo].[DaysInBattle](
                    @startdate date,
                    @enddate date
                )
                RETURNS int AS
                BEGIN
                    DECLARE @days int;
                    SELECT @days=datediff(D, @startdate, @enddate)+1;
                    RETURN @days;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION [Dbo].[DaysInBattle]");
        }
    }
}
