using Microsoft.EntityFrameworkCore.Migrations;

namespace FileSharing.Migrations
{
    public partial class addoriginalNAme_inupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrginalName",
                table: "Uploads",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrginalName",
                table: "Uploads");
        }
    }
}
