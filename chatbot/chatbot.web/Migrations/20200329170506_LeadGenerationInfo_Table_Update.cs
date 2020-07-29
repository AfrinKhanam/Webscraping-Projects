using Microsoft.EntityFrameworkCore.Migrations;

namespace UjjivanBank_ChatBOT.Migrations
{
    public partial class LeadGenerationInfo_Table_Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeadGenerationAction",
                table: "LeadGenerationInfos",
                newName: "LeadGenerationActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeadGenerationActionId",
                table: "LeadGenerationInfos",
                newName: "LeadGenerationAction");
        }
    }
}
