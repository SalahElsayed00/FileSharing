using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharing.Migrations
{
    public partial class adddownloadcount_indatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "Uploads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Uploads");
        }
    }
}
