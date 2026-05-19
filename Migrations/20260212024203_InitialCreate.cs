using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolPlatform.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                });

            migrationBuilder.CreateTable(
                name: "StudentMetrics",
                columns: table => new
                {
                    MetricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudyHours = table.Column<double>(type: "float", nullable: false),
                    Attendance = table.Column<double>(type: "float", nullable: false),
                    Resources = table.Column<double>(type: "float", nullable: false),
                    Extracurricular = table.Column<double>(type: "float", nullable: false),
                    Motivation = table.Column<double>(type: "float", nullable: false),
                    Internet = table.Column<double>(type: "float", nullable: false),
                    Age = table.Column<double>(type: "float", nullable: false),
                    OnlineCourses = table.Column<double>(type: "float", nullable: false),
                    Discussions = table.Column<double>(type: "float", nullable: false),
                    AssignmentCompletion = table.Column<double>(type: "float", nullable: false),
                    ExamScore = table.Column<double>(type: "float", nullable: false),
                    EduTech = table.Column<double>(type: "float", nullable: false),
                    StressLevel = table.Column<double>(type: "float", nullable: false),
                    FinalGrade = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMetrics", x => x.MetricId);
                    table.ForeignKey(
                        name: "FK_StudentMetrics_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherStudents",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherStudents", x => new { x.TeacherId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_TeacherStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherStudents_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentBehaviorPredictions",
                columns: table => new
                {
                    PredictionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    MetricId = table.Column<int>(type: "int", nullable: false),
                    ClusterId = table.Column<int>(type: "int", nullable: false),
                    ClusterLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PredictedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_StudentMetrics_StudentId",
                table: "StudentMetrics",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherStudents_StudentId",
                table: "TeacherStudents",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentBehaviorPredictions");

            migrationBuilder.DropTable(
                name: "TeacherStudents");

            migrationBuilder.DropTable(
                name: "StudentMetrics");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
