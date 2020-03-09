using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddFoodPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreferenceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodPreferences_FoodPreferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "FoodPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealsPreferences",
                columns: table => new
                {
                    MealId = table.Column<string>(nullable: false),
                    PreferenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealsPreferences", x => new { x.MealId, x.PreferenceId });
                    table.ForeignKey(
                        name: "FK_MealsPreferences_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealsPreferences_FoodPreferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "FoodPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodPreferences_PreferenceId",
                table: "FoodPreferences",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MealsPreferences_PreferenceId",
                table: "MealsPreferences",
                column: "PreferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealsPreferences");

            migrationBuilder.DropTable(
                name: "FoodPreferences");
        }
    }
}
