using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webappcloudrun.Migrations
{
    /// <inheritdoc />
    public partial class changingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "stars",
                schema: "main",
                newName: "stars");

            migrationBuilder.RenameTable(
                name: "ratings",
                schema: "main",
                newName: "ratings");

            migrationBuilder.RenameTable(
                name: "people",
                schema: "main",
                newName: "people");

            migrationBuilder.RenameTable(
                name: "movies",
                schema: "main",
                newName: "movies");

            migrationBuilder.RenameTable(
                name: "imageUrl",
                schema: "main",
                newName: "imageUrl");

            migrationBuilder.RenameTable(
                name: "FilmUser",
                schema: "main",
                newName: "FilmUser");

            migrationBuilder.RenameTable(
                name: "FilmAddict",
                schema: "main",
                newName: "FilmAddict");

            migrationBuilder.RenameTable(
                name: "FavMovies",
                schema: "main",
                newName: "FavMovies");

            migrationBuilder.RenameTable(
                name: "directors",
                schema: "main",
                newName: "directors");
        }
    }
}
