using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharing.Migrations
{
    public partial class addoriginalNAme_inupload2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Uploads");
        }
    }
}
