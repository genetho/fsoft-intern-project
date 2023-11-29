using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class updatelogintimeout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginAttemps",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginTimeOut",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1L,
                column: "LoginTimeOut",
                value: new DateTime(2022, 12, 19, 21, 32, 13, 802, DateTimeKind.Local).AddTicks(8581));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2L,
                column: "LoginTimeOut",
                value: new DateTime(2022, 12, 19, 21, 32, 13, 802, DateTimeKind.Local).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 3L,
                column: "LoginTimeOut",
                value: new DateTime(2022, 12, 19, 21, 32, 13, 802, DateTimeKind.Local).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 4L,
                column: "LoginTimeOut",
                value: new DateTime(2022, 12, 19, 21, 32, 13, 802, DateTimeKind.Local).AddTicks(8604));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginAttemps",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoginTimeOut",
                table: "Users");
        }
    }
}
