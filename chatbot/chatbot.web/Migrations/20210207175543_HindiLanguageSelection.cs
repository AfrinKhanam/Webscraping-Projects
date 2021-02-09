using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class HindiLanguageSelection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "WebPageLanguage_Id_Sequence",
                startValue: 3L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 23, 25, 43, 129, DateTimeKind.Local).AddTicks(5811),
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
                defaultValue: new DateTime(2021, 2, 7, 23, 25, 43, 128, DateTimeKind.Local).AddTicks(756),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 503, DateTimeKind.Local).AddTicks(6984));

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "StaticPages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WebPageLanguages",
                columns: table => new
                {
                    LanguageId = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"WebPageLanguage_Id_Sequence\"')"),
                    LanguageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPageLanguages", x => x.LanguageId);
                });

            migrationBuilder.InsertData(
                table: "WebPageLanguages",
                columns: new[] { "LanguageId", "LanguageName" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "Hindi" }
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

            migrationBuilder.DropTable(
                name: "WebPageLanguages");

            migrationBuilder.DropIndex(
                name: "IX_WebScrapeConfig_LanguageId",
                table: "WebScrapeConfig");

            migrationBuilder.DropIndex(
                name: "IX_Synonyms_LanguageId",
                table: "Synonyms");

            migrationBuilder.DropIndex(
                name: "IX_StaticPages_LanguageId",
                table: "StaticPages");

            migrationBuilder.DropSequence(
                name: "WebPageLanguage_Id_Sequence");

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
                oldDefaultValue: new DateTime(2021, 2, 7, 23, 25, 43, 129, DateTimeKind.Local).AddTicks(5811));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 31, 23, 14, 34, 503, DateTimeKind.Local).AddTicks(6984),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 2, 7, 23, 25, 43, 128, DateTimeKind.Local).AddTicks(756));
        }
    }
}
