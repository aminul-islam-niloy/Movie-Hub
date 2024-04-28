using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesFair.Data.Migrations
{
    public partial class ReviewandrRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.AddColumn<double>(
                name: "OverallRating",
                table: "Movies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Sourcelink",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalReviews",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverallRating",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Sourcelink",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "TotalReviews",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "Title");
        }
    }
}
