using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class StaticPageIsActiveToTrueByDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebScapeConfig",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 55, DateTimeKind.Local).AddTicks(7180),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 820, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "StaticPages",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 53, DateTimeKind.Local).AddTicks(2769),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 818, DateTimeKind.Local).AddTicks(2812));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebScapeConfig",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 820, DateTimeKind.Local).AddTicks(8212),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 55, DateTimeKind.Local).AddTicks(7180));

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "StaticPages",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 18, 23, 32, 818, DateTimeKind.Local).AddTicks(2812),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 53, DateTimeKind.Local).AddTicks(2769));
        }
    }
}
