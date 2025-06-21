using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class grigoras_added_seed_roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id", "fdf0262c-3db3-40b2-8bb9-867571bb1579", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator with full access", "Admin", "ADMIN" },
                    { "creator-role-id", "a7a7b16a-3786-41b4-95bd-c51c6df848bd", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Content creator with special permissions", "Creator", "CREATOR" },
                    { "user-role-id", "48c402d4-2a1f-45f9-be18-d6c5c8c5d46e", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Regular user with limited access", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id");
        }
    }
}
