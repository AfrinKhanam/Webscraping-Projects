using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UjjivanBank_ChatBOT.Migrations
{
    public partial class StaticPageUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 820, DateTimeKind.Local).AddTicks(8212),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 828, DateTimeKind.Local).AddTicks(6932));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 818, DateTimeKind.Local).AddTicks(2812),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 825, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "StaticPages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StaticPages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StaticPages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 828, DateTimeKind.Local).AddTicks(6932),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 820, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 825, DateTimeKind.Local).AddTicks(7133),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 818, DateTimeKind.Local).AddTicks(2812));
        }
    }
}
