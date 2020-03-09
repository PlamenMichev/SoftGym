using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class ChangePreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodPreferences_FoodPreferences_PreferenceId",
                table: "FoodPreferences");

            migrationBuilder.DropIndex(
                name: "IX_FoodPreferences_PreferenceId",
                table: "FoodPreferences");

            migrationBuilder.DropColumn(
                name: "PreferenceId",
                table: "FoodPreferences");

            migrationBuilder.AddColumn<int>(
                name: "Preference",
                table: "FoodPreferences",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preference",
                table: "FoodPreferences");

            migrationBuilder.AddColumn<int>(
                name: "PreferenceId",
                table: "FoodPreferences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodPreferences_PreferenceId",
                table: "FoodPreferences",
                column: "PreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodPreferences_FoodPreferences_PreferenceId",
                table: "FoodPreferences",
                column: "PreferenceId",
                principalTable: "FoodPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
