using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class DimensionFacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
