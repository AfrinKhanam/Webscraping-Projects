using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class AddedLanguageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticPages_WebPageLanguage_LanguageId",
                table: "StaticPages");

            migrationBuilder.DropForeignKey(
                name: "FK_Synonyms_WebPageLanguage_LanguageId",
                table: "Synonyms");

            migrationBuilder.DropForeignKey(
                name: "FK_WebScrapeConfig_WebPageLanguage_LanguageId",
                table: "WebScrapeConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebPageLanguage",
                table: "WebPageLanguage");

            migrationBuilder.RenameTable(
                name: "WebPageLanguage",
                newName: "WebPageLanguages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 30, 18, 45, 32, 630, DateTimeKind.Local).AddTicks(7428),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 14, DateTimeKind.Local).AddTicks(4056));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 30, 18, 45, 32, 628, DateTimeKind.Local).AddTicks(6013),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 12, DateTimeKind.Local).AddTicks(8243));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebPageLanguages",
                table: "WebPageLanguages",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticPages_WebPageLanguages_LanguageId",
                table: "StaticPages",
                column: "LanguageId",
                principalTable: "WebPageLanguages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Synonyms_WebPageLanguages_LanguageId",
                table: "Synonyms",
                column: "LanguageId",
                principalTable: "WebPageLanguages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebScrapeConfig_WebPageLanguages_LanguageId",
                table: "WebScrapeConfig",
                column: "LanguageId",
                principalTable: "WebPageLanguages",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaticPages_WebPageLanguages_LanguageId",
                table: "StaticPages");

            migrationBuilder.DropForeignKey(
                name: "FK_Synonyms_WebPageLanguages_LanguageId",
                table: "Synonyms");

            migrationBuilder.DropForeignKey(
                name: "FK_WebScrapeConfig_WebPageLanguages_LanguageId",
                table: "WebScrapeConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebPageLanguages",
                table: "WebPageLanguages");

            migrationBuilder.RenameTable(
                name: "WebPageLanguages",
                newName: "WebPageLanguage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 14, DateTimeKind.Local).AddTicks(4056),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 30, 18, 45, 32, 630, DateTimeKind.Local).AddTicks(7428));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 12, DateTimeKind.Local).AddTicks(8243),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 30, 18, 45, 32, 628, DateTimeKind.Local).AddTicks(6013));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebPageLanguage",
                table: "WebPageLanguage",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaticPages_WebPageLanguage_LanguageId",
                table: "StaticPages",
                column: "LanguageId",
                principalTable: "WebPageLanguage",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Synonyms_WebPageLanguage_LanguageId",
                table: "Synonyms",
                column: "LanguageId",
                principalTable: "WebPageLanguage",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WebScrapeConfig_WebPageLanguage_LanguageId",
                table: "WebScrapeConfig",
                column: "LanguageId",
                principalTable: "WebPageLanguage",
                principalColumn: "LanguageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
