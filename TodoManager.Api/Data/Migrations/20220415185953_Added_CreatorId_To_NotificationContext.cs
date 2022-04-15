using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoManager.Api.Data.Migrations
{
    public partial class Added_CreatorId_To_NotificationContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TodoCreatorId",
                table: "Notifications",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodoCreatorId",
                table: "Notifications");
        }
    }
}
