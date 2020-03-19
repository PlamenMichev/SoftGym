using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGym.Data.Migrations
{
    public partial class AddUserToWorkoutPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_AspNetUsers_ApplicationUserId",
                table: "WorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlans_ApplicationUserId",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WorkoutPlans");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkoutPlans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "WorkoutPlans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_AspNetUsers_UserId",
                table: "WorkoutPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_AspNetUsers_UserId",
                table: "WorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkoutPlans");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WorkoutPlans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_ApplicationUserId",
                table: "WorkoutPlans",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_AspNetUsers_ApplicationUserId",
                table: "WorkoutPlans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
