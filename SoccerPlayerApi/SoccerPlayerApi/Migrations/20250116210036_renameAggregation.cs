using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class renameAggregation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DimensionFact_DimensionValues_DimensionValueId",
                table: "DimensionFact");

            migrationBuilder.DropForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DimensionValues",
                table: "DimensionValues");

            migrationBuilder.RenameTable(
                name: "DimensionValues",
                newName: "Aggregations");

            migrationBuilder.RenameIndex(
                name: "IX_DimensionValues_LevelId",
                table: "Aggregations",
                newName: "IX_Aggregations_LevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aggregations",
                table: "Aggregations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aggregations_Levels_LevelId",
                table: "Aggregations",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionFact_Aggregations_DimensionValueId",
                table: "DimensionFact",
                column: "DimensionValueId",
                principalTable: "Aggregations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aggregations_Levels_LevelId",
                table: "Aggregations");

            migrationBuilder.DropForeignKey(
                name: "FK_DimensionFact_Aggregations_DimensionValueId",
                table: "DimensionFact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aggregations",
                table: "Aggregations");

            migrationBuilder.RenameTable(
                name: "Aggregations",
                newName: "DimensionValues");

            migrationBuilder.RenameIndex(
                name: "IX_Aggregations_LevelId",
                table: "DimensionValues",
                newName: "IX_DimensionValues_LevelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DimensionValues",
                table: "DimensionValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionFact_DimensionValues_DimensionValueId",
                table: "DimensionFact",
                column: "DimensionValueId",
                principalTable: "DimensionValues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DimensionValues_Levels_LevelId",
                table: "DimensionValues",
                column: "LevelId",
                principalTable: "Levels",
                principalColumn: "Id");
        }
    }
}
