using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    public partial class universe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DimensionIdFilter1 = table.Column<int>(type: "int", nullable: false),
                    LevelIdFilter1 = table.Column<int>(type: "int", nullable: false),
                    DimensionIdFilter2 = table.Column<int>(type: "int", nullable: false),
                    LevelIdFilter2 = table.Column<int>(type: "int", nullable: false),
                    DimensionIdFilter3 = table.Column<int>(type: "int", nullable: false),
                    LevelIdFilter3 = table.Column<int>(type: "int", nullable: false),
                    DimensionIdFilter4 = table.Column<int>(type: "int", nullable: false),
                    LevelIdFilter4 = table.Column<int>(type: "int", nullable: false),
                    DimensionIdFilter5 = table.Column<int>(type: "int", nullable: false),
                    LevelIdFilter5 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Environments");
        }
    }
}
