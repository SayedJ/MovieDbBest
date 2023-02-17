using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webappcloudrun.Migrations
{
    /// <inheritdoc />
    public partial class GoodglDatase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "directors");

            migrationBuilder.DropTable(
                name: "FavMovies");

            migrationBuilder.DropTable(
                name: "FilmAddict");

            migrationBuilder.DropTable(
                name: "FilmUser");

            migrationBuilder.DropTable(
                name: "imageUrl");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "stars");
        }
    }
}
