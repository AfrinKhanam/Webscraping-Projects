using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IndianBank_ChatBOT.Migrations
{
    public partial class addedUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "User_Id_Sequence",
                startValue: 2L);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 27, 11, 54, 16, 296, DateTimeKind.Local).AddTicks(2327),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 55, DateTimeKind.Local).AddTicks(7180));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 27, 11, 54, 16, 292, DateTimeKind.Local).AddTicks(7685),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 53, DateTimeKind.Local).AddTicks(2769));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"User_Id_Sequence\"')"),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password", "Username" },
                values: new object[] { 1, "Admin", "1agNZaFS+rqXmkl8Ewth1UQJq4le7RcFftwdBbrYf4FykftFJhXV11hPrX5d9YTtlv+9gXiJ6lB1Q44qpuGsow==", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "User_Id_Sequence");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "WebScapeConfig",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 55, DateTimeKind.Local).AddTicks(7180),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 8, 27, 11, 54, 16, 296, DateTimeKind.Local).AddTicks(2327));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "StaticPages",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 19, 19, 27, 42, 53, DateTimeKind.Local).AddTicks(2769),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 8, 27, 11, 54, 16, 292, DateTimeKind.Local).AddTicks(7685));
        }
    }
}
