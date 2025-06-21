using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class grigoras_added_role_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationRole",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id", "62d09376-f851-4642-aaf5-9f715a61294b", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator with full access", "Admin", "ADMIN" },
                    { "creator-role-id", "75053007-c75d-477c-93a9-145c11c3e131", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Content creator with special permissions", "Creator", "CREATOR" },
                    { "user-role-id", "ed131aaf-a8d8-4d09-855c-fbee6ecd1173", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Regular user with limited access", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationRole");
        }
    }
}
