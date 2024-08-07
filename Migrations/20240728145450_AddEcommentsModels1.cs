using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorBlog.Migrations
{
    public partial class AddEcommentsModels1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VotelistdwnJson",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VotelistupJson",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotelistdwnJson",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "VotelistupJson",
                table: "Comments");
        }
    }
}
