using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddEntitiesInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Visits = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientsTrainers",
                columns: table => new
                {
                    ClientId = table.Column<string>(nullable: false),
                    TrainerId = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsTrainers", x => new { x.ClientId, x.TrainerId });
                    table.ForeignKey(
                        name: "FK_ClientsTrainers_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientsTrainers_AspNetUsers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EatingPlans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    CaloriesPerDay = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EatingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EatingPlans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    VideoUrl = table.Column<string>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CaloriesPer100Grams = table.Column<double>(nullable: false),
                    PictureUrl = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    DaysInWeek = table.Column<int>(nullable: false),
                    Difficulty = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealsPlans",
                columns: table => new
                {
                    EatingPlanId = table.Column<string>(nullable: false),
                    MealId = table.Column<string>(nullable: false),
                    MealWeight = table.Column<double>(nullable: false),
                    TotalCalories = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealsPlans", x => new { x.MealId, x.EatingPlanId });
                    table.ForeignKey(
                        name: "FK_MealsPlans_EatingPlans_EatingPlanId",
                        column: x => x.EatingPlanId,
                        principalTable: "EatingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealsPlans_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutsExercises",
                columns: table => new
                {
                    ExerciseId = table.Column<string>(nullable: false),
                    WorkoutPlanId = table.Column<string>(nullable: false),
                    MinRepsCount = table.Column<int>(nullable: false),
                    MaxRepsCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutsExercises", x => new { x.WorkoutPlanId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_WorkoutsExercises_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutsExercises_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers",
                column: "CardId",
                unique: true,
                filter: "[CardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_IsDeleted",
                table: "Cards",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsTrainers_IsDeleted",
                table: "ClientsTrainers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsTrainers_TrainerId",
                table: "ClientsTrainers",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_EatingPlans_IsDeleted",
                table: "EatingPlans",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_EatingPlans_UserId",
                table: "EatingPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_IsDeleted",
                table: "Exercise",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_IsDeleted",
                table: "Facilities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_IsDeleted",
                table: "Meals",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MealsPlans_EatingPlanId",
                table: "MealsPlans",
                column: "EatingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_ApplicationUserId",
                table: "WorkoutPlans",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_IsDeleted",
                table: "WorkoutPlans",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutsExercises_ExerciseId",
                table: "WorkoutsExercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cards_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "ClientsTrainers");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "MealsPlans");

            migrationBuilder.DropTable(
                name: "WorkoutsExercises");

            migrationBuilder.DropTable(
                name: "EatingPlans");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
