using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class AddedNewColumnToStaticPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 6, 14, 53, 7, 798, DateTimeKind.Local).AddTicks(2622),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 6, 8, 22, 23, 14, 377, DateTimeKind.Local).AddTicks(8883));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastScrapedOn",
                table: "StaticPages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastScrapedOn",
                table: "StaticPages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 6, 8, 22, 23, 14, 377, DateTimeKind.Local).AddTicks(8883),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 6, 14, 53, 7, 798, DateTimeKind.Local).AddTicks(2622));
        }
    }
}
