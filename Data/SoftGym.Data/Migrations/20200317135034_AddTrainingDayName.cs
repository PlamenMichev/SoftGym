using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddTrainingDayName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsExercises_Exercise_ExerciseId",
                table: "WorkoutsExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsExercises_TrainingDays_TrainingDayId",
                table: "WorkoutsExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises");

            migrationBuilder.RenameTable(
                name: "WorkoutsExercises",
                newName: "WorkoutsTrainingDays");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutsExercises_ExerciseId",
                table: "WorkoutsTrainingDays",
                newName: "IX_WorkoutsTrainingDays_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutsTrainingDays",
                table: "WorkoutsTrainingDays",
                columns: new[] { "TrainingDayId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercise_ExerciseId",
                table: "WorkoutsTrainingDays",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsTrainingDays_TrainingDays_TrainingDayId",
                table: "WorkoutsTrainingDays",
                column: "TrainingDayId",
                principalTable: "TrainingDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsTrainingDays_Exercise_ExerciseId",
                table: "WorkoutsTrainingDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsTrainingDays_TrainingDays_TrainingDayId",
                table: "WorkoutsTrainingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutsTrainingDays",
                table: "WorkoutsTrainingDays");

            migrationBuilder.RenameTable(
                name: "WorkoutsTrainingDays",
                newName: "WorkoutsExercises");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutsTrainingDays_ExerciseId",
                table: "WorkoutsExercises",
                newName: "IX_WorkoutsExercises_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises",
                columns: new[] { "TrainingDayId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsExercises_Exercise_ExerciseId",
                table: "WorkoutsExercises",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsExercises_TrainingDays_TrainingDayId",
                table: "WorkoutsExercises",
                column: "TrainingDayId",
                principalTable: "TrainingDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
