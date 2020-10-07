using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class LanguageTableUpdated : Migration
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
                defaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 14, DateTimeKind.Local).AddTicks(4056),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 498, DateTimeKind.Local).AddTicks(2856));

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "WebPageLanguage",
                nullable: false,
                defaultValueSql: "nextval('\"WebPageLanguage_Id_Sequence\"')",
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 12, DateTimeKind.Local).AddTicks(8243),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 496, DateTimeKind.Local).AddTicks(7861));

            migrationBuilder.InsertData(
                table: "WebPageLanguage",
                columns: new[] { "LanguageId", "LanguageName" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "Hindi" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "WebPageLanguage_Id_Sequence");

            migrationBuilder.DeleteData(
                table: "WebPageLanguage",
                keyColumn: "LanguageId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WebPageLanguage",
                keyColumn: "LanguageId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScrapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 498, DateTimeKind.Local).AddTicks(2856),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 14, DateTimeKind.Local).AddTicks(4056));

            migrationBuilder.AlterColumn<int>(
                name: "LanguageId",
                table: "WebPageLanguage",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "nextval('\"WebPageLanguage_Id_Sequence\"')")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 24, 16, 40, 28, 496, DateTimeKind.Local).AddTicks(7861),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 24, 16, 51, 32, 12, DateTimeKind.Local).AddTicks(8243));
        }
    }
}
