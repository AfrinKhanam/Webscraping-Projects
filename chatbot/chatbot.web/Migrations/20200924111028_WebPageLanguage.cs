using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class WebPageLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 498, DateTimeKind.Local).AddTicks(2856),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 507, DateTimeKind.Local).AddTicks(4471));

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "WebScrapeConfig",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Synonyms",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 496, DateTimeKind.Local).AddTicks(7861),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 503, DateTimeKind.Local).AddTicks(6984));

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "StaticPages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WebPageLanguage",
                columns: table => new
                {
                    LanguageId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPageLanguage", x => x.LanguageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebScrapeConfig_LanguageId",
                table: "WebScrapeConfig",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_LanguageId",
                table: "Synonyms",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticPages_LanguageId",
                table: "StaticPages",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "WebPageLanguage");

            migrationBuilder.DropIndex(
                name: "IX_WebScrapeConfig_LanguageId",
                table: "WebScrapeConfig");

            migrationBuilder.DropIndex(
                name: "IX_Synonyms_LanguageId",
                table: "Synonyms");

            migrationBuilder.DropIndex(
                name: "IX_StaticPages_LanguageId",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "WebScrapeConfig");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Synonyms");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "StaticPages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 507, DateTimeKind.Local).AddTicks(4471),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 498, DateTimeKind.Local).AddTicks(2856));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 503, DateTimeKind.Local).AddTicks(6984),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 496, DateTimeKind.Local).AddTicks(7861));
        }
    }
}
