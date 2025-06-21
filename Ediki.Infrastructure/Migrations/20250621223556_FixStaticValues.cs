using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStaticValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "ConcurrencyStamp",
                value: "8f434346-85c4-4c5b-b7e8-4c5e8b7f9d1c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id",
                column: "ConcurrencyStamp",
                value: "6c421130-63b2-2a39-95e6-2a3d6b7e9c1f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "ConcurrencyStamp",
                value: "7d532240-74c3-3b4a-a6f7-3b4e7c8f0e2d");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "ConcurrencyStamp",
                value: "53bc29b4-8b78-44e3-8337-6b3dfa88dc99");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id",
                column: "ConcurrencyStamp",
                value: "3e459d1d-bfe4-4efa-8624-3689d04a07ae");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "ConcurrencyStamp",
                value: "8c915a79-d638-4100-ba0b-aced9c321f95");
        }
    }
}
