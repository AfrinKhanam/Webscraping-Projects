using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class StaticPageModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Html",
                table: "StaticPages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "StaticPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "StaticPages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "FileData",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "StaticPages");

            migrationBuilder.AddColumn<string>(
                name: "Html",
                table: "StaticPages",
                type: "text",
                maxLength: 2147483647,
                nullable: true);
        }
    }
}
