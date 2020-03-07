using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddTypeToFacility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Facilities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Facilities");
        }
    }
}
