using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Migrations;

namespace UjjivanBank_ChatBOT.Migrations
{
    public partial class StaticPageModelChages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncodedPageUrl",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "PageUrl",
                table: "StaticPages");

            migrationBuilder.AddColumn<string>(
                name: "Html",
                table: "StaticPages",
                maxLength: 2147483647,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScrapeStatus",
                table: "StaticPages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatBotVisitorDetails",
                columns: table => new
                {
                    Visitor = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    NumberOfVisits = table.Column<int>(nullable: false),
                    NumberOfQueries = table.Column<int>(nullable: false),
                    LastVisited = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FrequentlyAskedQueries",
                columns: table => new
                {
                    ActivityIds = table.Column<List<string>>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Query = table.Column<string>(nullable: true),
                    PositiveFeedback = table.Column<int>(nullable: false),
                    NegetiveFeedback = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Top10DomainsVisitedViewModels",
                columns: table => new
                {
                    DomainName = table.Column<string>(nullable: true),
                    TotalHits = table.Column<int>(nullable: false),
                    HitPercentage = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UnAnsweredQueries",
                columns: table => new
                {
                    PhoneNumber = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Query = table.Column<string>(nullable: true),
                    BotResponse = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "VisitorsByMonthViewModels",
                columns: table => new
                {
                    DayNumber = table.Column<int>(nullable: false),
                    MonthText = table.Column<string>(nullable: true),
                    Visits = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "VisitorsByYearMonthViewModels",
                columns: table => new
                {
                    MonthNumber = table.Column<int>(nullable: false),
                    MonthText = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Visits = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatBotVisitorDetails");

            migrationBuilder.DropTable(
                name: "FrequentlyAskedQueries");

            migrationBuilder.DropTable(
                name: "Top10DomainsVisitedViewModels");

            migrationBuilder.DropTable(
                name: "UnAnsweredQueries");

            migrationBuilder.DropTable(
                name: "VisitorsByMonthViewModels");

            migrationBuilder.DropTable(
                name: "VisitorsByYearMonthViewModels");

            migrationBuilder.DropColumn(
                name: "Html",
                table: "StaticPages");

            migrationBuilder.DropColumn(
                name: "ScrapeStatus",
                table: "StaticPages");

            migrationBuilder.AddColumn<string>(
                name: "EncodedPageUrl",
                table: "StaticPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageUrl",
                table: "StaticPages",
                nullable: true);
        }
    }
}
