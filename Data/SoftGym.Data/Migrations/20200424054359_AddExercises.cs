using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddExercises : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercise_ExerciseId",
                table: "WorkoutsTrainingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_IsDeleted",
                table: "Exercises",
                newName: "IX_Exercises_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercises_ExerciseId",
                table: "WorkoutsTrainingDays",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercises_ExerciseId",
                table: "WorkoutsTrainingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_IsDeleted",
                table: "Exercise",
                newName: "IX_Exercise_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercise_ExerciseId",
                table: "WorkoutsTrainingDays",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
