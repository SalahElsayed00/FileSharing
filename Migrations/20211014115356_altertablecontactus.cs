using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharing.Migrations
{
    public partial class altertablecontactus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Contacts",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contacts");
        }
    }
}
