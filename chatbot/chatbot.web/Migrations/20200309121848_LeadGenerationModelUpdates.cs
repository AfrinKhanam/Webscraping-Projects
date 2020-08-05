using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class LeadGenerationModelUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserInfo",
                table: "LeadGenerationInfos",
                newName: "UserInfoId");

            migrationBuilder.AlterColumn<int>(
                name: "LeadGenerationAction",
                table: "LeadGenerationInfos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "LeadGenerationInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "LeadGenerationInfos");

            migrationBuilder.RenameColumn(
                name: "UserInfoId",
                table: "LeadGenerationInfos",
                newName: "UserInfo");

            migrationBuilder.AlterColumn<int>(
                name: "LeadGenerationAction",
                table: "LeadGenerationInfos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
