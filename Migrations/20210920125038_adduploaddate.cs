using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FileSharing.Migrations
{
    public partial class adduploaddate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Uploads",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getDate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Uploads");
        }
    }
}
