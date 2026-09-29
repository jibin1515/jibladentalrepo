using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_remaining_banner_alt_fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "banner_image_alt",
                table: "page_settings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "banner_image_alt",
                table: "news",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "banner_image_alt",
                table: "loyalty",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "banner_image_alt",
                table: "service",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "testimonial_image_alt",
                table: "about",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banner_image_alt",
                table: "page_settings");

            migrationBuilder.DropColumn(
                name: "banner_image_alt",
                table: "news");

            migrationBuilder.DropColumn(
                name: "banner_image_alt",
                table: "loyalty");

            migrationBuilder.DropColumn(
                name: "banner_image_alt",
                table: "service");

            migrationBuilder.DropColumn(
                name: "testimonial_image_alt",
                table: "about");
        }
    }
}
