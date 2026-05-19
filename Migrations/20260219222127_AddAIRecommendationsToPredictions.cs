using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPlatform.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAIRecommendationsToPredictions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LearningStyle",
                table: "StudentBehaviorPredictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizLevel",
                table: "StudentBehaviorPredictions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecoveryPlan",
                table: "StudentBehaviorPredictions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LearningStyle",
                table: "StudentBehaviorPredictions");

            migrationBuilder.DropColumn(
                name: "QuizLevel",
                table: "StudentBehaviorPredictions");

            migrationBuilder.DropColumn(
                name: "RecoveryPlan",
                table: "StudentBehaviorPredictions");
        }
    }
}
