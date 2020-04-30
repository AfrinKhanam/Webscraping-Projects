using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class Added_WebPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "WebPageScrapeRequests");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "WebPageScrapeRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "WebPageScrapeRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDate",
                table: "WebPageScrapeRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WebPageId",
                table: "WebPageScrapeRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "WebPages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "WebPages",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebPageScrapeRequests_WebPageId",
                table: "WebPageScrapeRequests",
                column: "WebPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebPageScrapeRequests_WebPages_WebPageId",
                table: "WebPageScrapeRequests",
                column: "WebPageId",
                principalTable: "WebPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebPageScrapeRequests_WebPages_WebPageId",
                table: "WebPageScrapeRequests");

            migrationBuilder.DropIndex(
                name: "IX_WebPageScrapeRequests_WebPageId",
                table: "WebPageScrapeRequests");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "WebPageScrapeRequests");

            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "WebPageScrapeRequests");

            migrationBuilder.DropColumn(
                name: "WebPageId",
                table: "WebPageScrapeRequests");

            migrationBuilder.AddColumn<string>(
                name: "MyProperty",
                table: "WebPageScrapeRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "WebPageScrapeRequests",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "WebPages",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "PageName",
                table: "WebPages",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
