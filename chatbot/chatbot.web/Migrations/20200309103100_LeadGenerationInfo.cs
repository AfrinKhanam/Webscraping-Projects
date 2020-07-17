using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class LeadGenerationInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadGenerationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Visitor = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    QueriedOn = table.Column<DateTime>(nullable: false),
                    DomainName = table.Column<string>(nullable: true),
                    UserInfo = table.Column<int>(nullable: false),
                    LeadGenerationAction = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadGenerationInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadGenerationInfos");
        }
    }
}
