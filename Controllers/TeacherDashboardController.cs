using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolPlatform.Api.Data;
using SchoolPlatform.Api.Models;

namespace SchoolPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/teacher")]
    public class TeacherDashboardController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TeacherDashboardController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] int teacherId)
        {
            var studentIds = await _db.TeacherStudents
                .Where(ts => ts.TeacherId == teacherId)
                .Select(ts => ts.StudentId)
                .ToListAsync();

            if (studentIds.Count == 0)
            {
                return Ok(new
                {
                    teacherId,
                    studentCount = 0,
                    levelCounts = new List<object>(),
                    avgByLevel = new List<object>(),
                    students = new List<object>()
                });
            }

            var students = await _db.Students
                .Where(s => studentIds.Contains(s.StudentId))
                .Include(s => s.Metrics)
                .Include(s => s.Predictions)
                .ToListAsync();

            var overview = students.Select(s =>
            {
                var latestMetric = s.Metrics
                    .OrderByDescending(x => x.RecordedAt)
                    .FirstOrDefault();

                var lastPrediction = s.Predictions
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefault();

                return new
                {
                    studentId = s.StudentId,
                    fullName = s.FullName,
                    gradeLevel = s.GradeLevel,
                    attendance = latestMetric?.AttendancePercentage ?? 0,
                    studyHours = latestMetric?.StudyHours ?? 0,
                    predictedScore = lastPrediction?.PredictedScore ?? 0,
           
                    level = lastPrediction?.Level ?? "Unknown"
                };
            }).ToList();

            //Student Distribution
            var levelCounts = overview
                .GroupBy(x => x.level)
                .Select(g => new
                {
                    level = g.Key,
                    count = g.Count()
                })
                .ToList();

            // Averages
            var avgByLevel = overview
                .GroupBy(x => x.level)
                .Select(g => new
                {
                    level = g.Key,
                    avgScore = g.Average(x => x.predictedScore),
                    avgAttendance = g.Average(x => x.attendance)
                })
                .ToList();

            return Ok(new
            {
                teacherId,
                studentCount = students.Count,
                levelCounts,
                avgByLevel,
                students = overview
            });
        }
    }
}