using Microsoft.EntityFrameworkCore.Migrations;

namespace BellumGens.Api.Core.Migrations
{
    public partial class TwitchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwitchId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "Languages",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Languages", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ApplicationUserLanguages",
            //    columns: table => new
            //    {
            //        LanguagesSpokenId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ApplicationUserLanguages", x => new { x.LanguagesSpokenId, x.UsersId });
            //        table.ForeignKey(
            //            name: "FK_ApplicationUserLanguages_AspNetUsers_UsersId",
            //            column: x => x.UsersId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ApplicationUserLanguages_Languages_LanguagesSpokenId",
            //            column: x => x.LanguagesSpokenId,
            //            principalTable: "Languages",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ApplicationUserLanguages_UsersId",
            //    table: "ApplicationUserLanguages",
            //    column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ApplicationUserLanguages");

            //migrationBuilder.DropTable(
            //    name: "Languages");

            migrationBuilder.DropColumn(
                name: "TwitchId",
                table: "AspNetUsers");
        }
    }
}
