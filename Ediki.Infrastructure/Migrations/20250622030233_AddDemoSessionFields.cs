using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoSessionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "DemoSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "DemoSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireModeration",
                table: "DemoSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Settings_AllowScreenShare",
                table: "DemoSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "DemoSessions");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "DemoSessions");

            migrationBuilder.DropColumn(
                name: "RequireModeration",
                table: "DemoSessions");

            migrationBuilder.DropColumn(
                name: "Settings_AllowScreenShare",
                table: "DemoSessions");
        }
    }
}
