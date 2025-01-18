using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class universe4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Environments_LevelIdFilter1",
                table: "Environments",
                column: "LevelIdFilter1");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_LevelIdFilter2",
                table: "Environments",
                column: "LevelIdFilter2");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_LevelIdFilter3",
                table: "Environments",
                column: "LevelIdFilter3");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_LevelIdFilter4",
                table: "Environments",
                column: "LevelIdFilter4");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_LevelIdFilter5",
                table: "Environments",
                column: "LevelIdFilter5");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter1",
                table: "Environments",
                column: "LevelIdFilter1",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter2",
                table: "Environments",
                column: "LevelIdFilter2",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter3",
                table: "Environments",
                column: "LevelIdFilter3",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter4",
                table: "Environments",
                column: "LevelIdFilter4",
                principalTable: "Levels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter5",
                table: "Environments",
                column: "LevelIdFilter5",
                principalTable: "Levels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter1",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter2",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter3",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter4",
                table: "Environments");

            migrationBuilder.DropForeignKey(
                name: "FK_Environments_Levels_LevelIdFilter5",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_LevelIdFilter1",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_LevelIdFilter2",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_LevelIdFilter3",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_LevelIdFilter4",
                table: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_Environments_LevelIdFilter5",
                table: "Environments");
        }
    }
}
