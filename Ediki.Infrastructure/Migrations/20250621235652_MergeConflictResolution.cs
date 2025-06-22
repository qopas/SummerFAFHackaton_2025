using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeConflictResolution : Migration
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
                value: "9122b3b8-3e41-4872-8b4b-e2e1e102713f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id",
                column: "ConcurrencyStamp",
                value: "96de642f-6de1-4189-a014-2967a70a4466");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "ConcurrencyStamp",
                value: "99eaa8c5-5d6a-4dc7-9d3a-2db3b80984f2");
        }
    }
}
