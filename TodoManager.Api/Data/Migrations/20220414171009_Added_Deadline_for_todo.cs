using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoManager.Api.Data.Migrations
{
    public partial class Added_Deadline_for_todo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Todos",
                newName: "CreatorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Todos",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Todos",
                newName: "CreatedBy");
        }
    }
}
