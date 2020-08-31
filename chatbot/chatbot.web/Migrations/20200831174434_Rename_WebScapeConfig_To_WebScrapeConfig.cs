using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class Rename_WebScapeConfig_To_WebScrapeConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WebScapeConfig",
                table: "WebScapeConfig"
            );

            migrationBuilder.RenameTable(
                name: "WebScapeConfig",
                newName: "WebScrapeConfig");

            migrationBuilder.AddPrimaryKey("PK_WebScrapeConfig", "WebScrapeConfig", "Id");

            migrationBuilder.Sql(@"
ALTER TABLE ""WebScrapeConfig"" ALTER COLUMN ""PageConfig"" TYPE jsonb USING ""PageConfig""::JSON;
ALTER TABLE ""WebScrapeConfig"" ALTER COLUMN ""PageConfig"" SET NOT NULL;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WebScrapeConfig",
                table: "WebScrapeConfig"
            );

            migrationBuilder.RenameTable(
                name: "WebScrapeConfig",
                newName: "WebScapeConfig");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebScapeConfig",
                table: "WebScapeConfig",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PageConfig",
                table: "WebScrapeConfig",
                type: "text",
                oldType: "jsonb"
            );
        }
    }
}
