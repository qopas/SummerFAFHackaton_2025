using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedAt", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id", "8f434346-85c4-4c5b-b7e8-4c5e8b7f9d1c", new DateTime(2025, 6, 22, 0, 25, 17, 364, DateTimeKind.Utc).AddTicks(6600), "", "Admin", "ADMIN" },
                    { "creator-role-id", "6c421130-63b2-2a39-95e6-2a3d6b7e9c1f", new DateTime(2025, 6, 22, 0, 25, 17, 365, DateTimeKind.Utc).AddTicks(2290), "", "Creator", "CREATOR" },
                    { "user-role-id", "7d532240-74c3-3b4a-a6f7-3b4e7c8f0e2d", new DateTime(2025, 6, 22, 0, 25, 17, 365, DateTimeKind.Utc).AddTicks(2310), "", "User", "USER" }
                });
        }
    }
}
