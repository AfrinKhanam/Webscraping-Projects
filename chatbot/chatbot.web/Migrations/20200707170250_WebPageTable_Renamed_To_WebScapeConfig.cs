using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class WebPageTable_Renamed_To_WebScapeConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebPageScrapeRequests");

            migrationBuilder.DropTable(
                name: "WebPages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 825, DateTimeKind.Local).AddTicks(7133),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 6, 14, 53, 7, 798, DateTimeKind.Local).AddTicks(2622));

            migrationBuilder.CreateTable(
                name: "WebScapeConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(nullable: false),
                    PageName = table.Column<string>(nullable: false),
                    PageConfig = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 828, DateTimeKind.Local).AddTicks(6932)),
                    LastScrapedOn = table.Column<DateTime>(nullable: true),
                    ScrapeStatus = table.Column<int>(nullable: false),
                    ErrorMessage = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebScapeConfig", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebScapeConfig");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 6, 14, 53, 7, 798, DateTimeKind.Local).AddTicks(2622),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 7, 7, 22, 32, 49, 825, DateTimeKind.Local).AddTicks(7133));

            migrationBuilder.CreateTable(
                name: "WebPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PageName = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebPageScrapeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ScrapeStatus = table.Column<int>(type: "integer", nullable: false),
                    WebPageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebPageScrapeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebPageScrapeRequests_WebPages_WebPageId",
                        column: x => x.WebPageId,
                        principalTable: "WebPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebPageScrapeRequests_WebPageId",
                table: "WebPageScrapeRequests",
                column: "WebPageId");
        }
    }
}
