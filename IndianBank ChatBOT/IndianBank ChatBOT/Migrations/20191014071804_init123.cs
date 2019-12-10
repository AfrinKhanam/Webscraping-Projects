using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBankChatBOT.Migrations
{
    public partial class init123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityType = table.Column<string>(nullable: true),
                    ReplyToActivityId = table.Column<string>(nullable: true),
                    ConversationId = table.Column<string>(nullable: true),
                    ConversationType = table.Column<string>(nullable: true),
                    ConversationName = table.Column<string>(nullable: true),
                    FromId = table.Column<string>(nullable: true),
                    FromName = table.Column<string>(nullable: true),
                    RecipientId = table.Column<string>(nullable: true),
                    RecipientName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    RasaIntent = table.Column<string>(nullable: true),
                    RasaScore = table.Column<double>(nullable: false),
                    RasaEntities = table.Column<string>(nullable: true),
                    TimestampUtc = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Question = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatLog");

            migrationBuilder.DropTable(
                name: "Faq");
        }
    }
}
