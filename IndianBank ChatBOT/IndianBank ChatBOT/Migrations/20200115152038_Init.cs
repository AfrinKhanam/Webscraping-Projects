using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ResponseSource = table.Column<int>(nullable: true),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityType = table.Column<string>(nullable: true),
                    ConversationId = table.Column<string>(nullable: true),
                    ConversationType = table.Column<string>(nullable: true),
                    ConversationName = table.Column<string>(nullable: true),
                    ReplyToActivityId = table.Column<string>(nullable: true),
                    FromId = table.Column<string>(nullable: true),
                    FromName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    RecipientId = table.Column<string>(nullable: true),
                    MainTitle = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    RecipientName = table.Column<string>(nullable: true),
                    ResponseJsonText = table.Column<string>(nullable: true),
                    RasaIntent = table.Column<string>(nullable: true),
                    RasaScore = table.Column<double>(nullable: true),
                    RasaEntities = table.Column<string>(nullable: true),
                    ResonseFeedback = table.Column<int>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: true),
                    IsOnBoardingMessage = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticPages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EncodedPageUrl = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    PageConfig = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Synonyms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Word = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synonyms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ConversationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SynonymWords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    SynonymId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynonymWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SynonymWords_Synonyms_SynonymId",
                        column: x => x.SynonymId,
                        principalTable: "Synonyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SynonymWords_SynonymId",
                table: "SynonymWords",
                column: "SynonymId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatLogs");

            migrationBuilder.DropTable(
                name: "StaticPages");

            migrationBuilder.DropTable(
                name: "SynonymWords");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "Synonyms");
        }
    }
}
