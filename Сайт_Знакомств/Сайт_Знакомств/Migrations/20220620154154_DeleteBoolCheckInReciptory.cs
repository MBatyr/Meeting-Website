using Microsoft.EntityFrameworkCore.Migrations;

namespace Сайт_Знакомств.Migrations
{
    public partial class DeleteBoolCheckInReciptory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User1Connect",
                table: "Reciprocity");

            migrationBuilder.DropColumn(
                name: "User2Connect",
                table: "Reciprocity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "User1Connect",
                table: "Reciprocity",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "User2Connect",
                table: "Reciprocity",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
