using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddTrainingDayEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsExercises_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutsExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "WorkoutsExercises");

            migrationBuilder.AddColumn<string>(
                name: "TrainingDayId",
                table: "WorkoutsExercises",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises",
                columns: new[] { "TrainingDayId", "ExerciseId" });

            migrationBuilder.CreateTable(
                name: "TrainingDays",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    WorkoutPlanId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingDays_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingDays_WorkoutPlanId",
                table: "TrainingDays",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsExercises_TrainingDays_TrainingDayId",
                table: "WorkoutsExercises",
                column: "TrainingDayId",
                principalTable: "TrainingDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsExercises_TrainingDays_TrainingDayId",
                table: "WorkoutsExercises");

            migrationBuilder.DropTable(
                name: "TrainingDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises");

            migrationBuilder.DropColumn(
                name: "TrainingDayId",
                table: "WorkoutsExercises");

            migrationBuilder.AddColumn<string>(
                name: "WorkoutPlanId",
                table: "WorkoutsExercises",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutsExercises",
                table: "WorkoutsExercises",
                columns: new[] { "WorkoutPlanId", "ExerciseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsExercises_WorkoutPlans_WorkoutPlanId",
                table: "WorkoutsExercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
