using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UjjivanBank_ChatBOT.Migrations
{
    public partial class UpdatedDefaultValueForCreatedDateInStaticPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 22, 23, 14, 377, DateTimeKind.Local).AddTicks(8883),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 6, 8, 22, 23, 14, 377, DateTimeKind.Local).AddTicks(8883));
        }
    }
}
