using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webappcloudrun.Migrations
{
    /// <inheritdoc />
    public partial class changetodbo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

          

        

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "role",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "confirm_password",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "FavMovies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "movie_id",
                table: "FavMovies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.RenameTable(
                name: "stars",
                newName: "stars",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "ratings",
                newName: "ratings",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "people",
                newName: "people",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "movies",
                newName: "movies",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "imageUrl",
                newName: "imageUrl",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "FilmUser",
                newName: "FilmUser",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "FilmAddict",
                newName: "FilmAddict",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "FavMovies",
                newName: "FavMovies",
                newSchema: "main");

            migrationBuilder.RenameTable(
                name: "directors",
                newName: "directors",
                newSchema: "main");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                schema: "main",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role",
                schema: "main",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "main",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "main",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "main",
                table: "FilmUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                schema: "main",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "password",
                schema: "main",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "confirm_password",
                schema: "main",
                table: "FilmAddict",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                schema: "main",
                table: "FavMovies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "movie_id",
                schema: "main",
                table: "FavMovies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
