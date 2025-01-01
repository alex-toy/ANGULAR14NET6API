using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class facts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DimensionFact_Sales_FactId",
                table: "DimensionFact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sales",
                table: "Sales");

            migrationBuilder.RenameTable(
                name: "Sales",
                newName: "Facts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facts",
                table: "Facts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionFact_Facts_FactId",
                table: "DimensionFact",
                column: "FactId",
                principalTable: "Facts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DimensionFact_Facts_FactId",
                table: "DimensionFact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facts",
                table: "Facts");

            migrationBuilder.RenameTable(
                name: "Facts",
                newName: "Sales");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sales",
                table: "Sales",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionFact_Sales_FactId",
                table: "DimensionFact",
                column: "FactId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
