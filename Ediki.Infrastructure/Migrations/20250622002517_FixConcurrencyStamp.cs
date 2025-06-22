using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ediki.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixConcurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 25, 17, 364, DateTimeKind.Utc).AddTicks(6600));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 25, 17, 365, DateTimeKind.Utc).AddTicks(2290));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 25, 17, 365, DateTimeKind.Utc).AddTicks(2310));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 24, 48, 920, DateTimeKind.Utc).AddTicks(4280));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "creator-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 24, 48, 921, DateTimeKind.Utc).AddTicks(230));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id",
                column: "CreatedAt",
                value: new DateTime(2025, 6, 22, 0, 24, 48, 921, DateTimeKind.Utc).AddTicks(240));
        }
    }
}
