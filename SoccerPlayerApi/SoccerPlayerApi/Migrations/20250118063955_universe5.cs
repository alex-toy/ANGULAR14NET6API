using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class universe5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DimensionIdFilter1",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "DimensionIdFilter2",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "DimensionIdFilter3",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "DimensionIdFilter4",
                table: "Environments");

            migrationBuilder.DropColumn(
                name: "DimensionIdFilter5",
                table: "Environments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DimensionIdFilter1",
                table: "Environments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DimensionIdFilter2",
                table: "Environments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DimensionIdFilter3",
                table: "Environments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DimensionIdFilter4",
                table: "Environments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DimensionIdFilter5",
                table: "Environments",
                type: "int",
                nullable: true);
        }
    }
}
