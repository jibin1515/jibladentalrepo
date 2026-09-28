using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class abtupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "certificate3image_alt",
                table: "contact",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "certificate3image_path",
                table: "contact",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count1symbol",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count2symbol",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count3symbol",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "certificate3image_alt",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "certificate3image_path",
                table: "contact");

            migrationBuilder.DropColumn(
                name: "count1symbol",
                table: "about");

            migrationBuilder.DropColumn(
                name: "count2symbol",
                table: "about");

            migrationBuilder.DropColumn(
                name: "count3symbol",
                table: "about");
        }
    }
}
