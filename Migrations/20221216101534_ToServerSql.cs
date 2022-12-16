using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webappcloudrun.Migrations
{
    /// <inheritdoc />
    public partial class ToServerSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "directors",
                columns: table => new
                {
                    movieid = table.Column<int>(name: "movie_id", type: "int", nullable: true),
                    personid = table.Column<int>(name: "person_id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FavMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(name: "user_id", type: "int", nullable: false),
                    movieid = table.Column<int>(name: "movie_id", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavMovies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilmAddict",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    confirmpassword = table.Column<string>(name: "confirm_password", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmAddict", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "FilmUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmUser", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "imageUrl",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movieid = table.Column<int>(name: "movie_id", type: "int", nullable: true),
                    imageurl = table.Column<string>(name: "image_url", type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imageUrl", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    birth = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    movieid = table.Column<int>(name: "movie_id", type: "int", nullable: true),
                    rating = table.Column<float>(type: "real", nullable: true),
                    votes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "stars",
                columns: table => new
                {
                    movieid = table.Column<int>(name: "movie_id", type: "int", nullable: true),
                    personid = table.Column<int>(name: "person_id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                });
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
