using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class BetterName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BetterName_CreatedAt",
                table: "Samurais",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                table: "Samurais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Samurais",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BetterName_UpdatedAt",
                table: "Samurais",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetterName_CreatedAt",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "GivenName",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "BetterName_UpdatedAt",
                table: "Samurais");
        }
    }
}
