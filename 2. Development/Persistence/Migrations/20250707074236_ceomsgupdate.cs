using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ceomsgupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ceo_image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count1image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count2image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "count3image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "home_image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "vision_mission_image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ceo_image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "count1image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "count2image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "count3image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "home_image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "image_alt",
                table: "about");

            migrationBuilder.DropColumn(
                name: "vision_mission_image_alt",
                table: "about");
        }
    }
}
