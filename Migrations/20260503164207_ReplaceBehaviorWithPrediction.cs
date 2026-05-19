using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPlatform.Api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceBehaviorWithPrediction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentBehaviorPredictions");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "AssignmentCompletion",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Attendance",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Discussions",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "EduTech",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "ExamScore",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Extracurricular",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "FinalGrade",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Internet",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Motivation",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "OnlineCourses",
                table: "StudentMetrics");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "StudentMetrics");

            migrationBuilder.RenameColumn(
                name: "StressLevel",
                table: "StudentMetrics",
                newName: "AttendancePercentage");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExtraActivities",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternetAccess",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolType",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudyMethod",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TravelTime",
                table: "Students",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "StudentPredictions",
                columns: table => new
                {
                    PredictionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PredictedScore = table.Column<double>(type: "float", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentPredictions", x => x.PredictionId);
                    table.ForeignKey(
                        name: "FK_StudentPredictions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentPredictions_StudentId",
                table: "StudentPredictions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentPredictions");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ExtraActivities",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "InternetAccess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudyMethod",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TravelTime",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "AttendancePercentage",
                table: "StudentMetrics",
                newName: "StressLevel");

            migrationBuilder.AddColumn<double>(
                name: "Age",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AssignmentCompletion",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Attendance",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Discussions",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EduTech",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExamScore",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Extracurricular",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FinalGrade",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Internet",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Motivation",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OnlineCourses",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Resources",
                table: "StudentMetrics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "StudentBehaviorPredictions",
                columns: table => new
                {
                    PredictionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetricId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClusterId = table.Column<int>(type: "int", nullable: false),
                    ClusterLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LearningStyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PredictedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuizLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecoveryPlan = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentBehaviorPredictions", x => x.PredictionId);
                    table.ForeignKey(
                        name: "FK_StudentBehaviorPredictions_StudentMetrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "StudentMetrics",
                        principalColumn: "MetricId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentBehaviorPredictions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentBehaviorPredictions_MetricId",
                table: "StudentBehaviorPredictions",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBehaviorPredictions_StudentId",
                table: "StudentBehaviorPredictions",
                column: "StudentId");
        }
    }
}
