using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolPlatform.Api.Data;
using SchoolPlatform.Api.Models;
using System;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace SchoolPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentDashboardController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _http;

        public StudentDashboardController(AppDbContext db, IHttpClientFactory http)
        {
            _db = db;
            _http = http;
        }


        // Endpoint to get the dashboard data for a student
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] int studentId)
        {
            var student = await _db.Students
                .Include(s => s.Metrics)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
                return NotFound("Student not found");

            var latestMetric = student.Metrics
                .OrderByDescending(m => m.RecordedAt)
                .FirstOrDefault();

            if (latestMetric == null)
                return NotFound("No metrics found");


            var lastQuiz = await _db.StudentQuizAssignments
                .Where(q => q.StudentId == studentId && q.IsSubmitted)
                .OrderByDescending(q => q.SubmittedAt)
                .FirstOrDefaultAsync();


            // history of the chart
            var history = student.Metrics
            .OrderBy(m => m.RecordedAt)
            .Select(m => new
            {
                date = m.RecordedAt.ToString("yyyy-MM"),
                studyHours = m.StudyHours,
                attendance = m.AttendancePercentage,
                examScore = m.ExamScore
            })
            .ToList();

            var client = _http.CreateClient("AiService");

            var aiPayload = new
            {
                age = student.Age,
                gender = student.Gender,
                school_type = student.SchoolType,
                study_hours = latestMetric.StudyHours,
                attendance_percentage = latestMetric.AttendancePercentage,
                internet_access = student.InternetAccess,
                travel_time = student.TravelTime,
                extra_activities = student.ExtraActivities,
                study_method = student.StudyMethod
            };


            var aiResponse = await client.PostAsJsonAsync("/api/predict/explain", aiPayload);

            ExplainResult? explain = null;

            if (aiResponse.IsSuccessStatusCode)
            {
                var raw = await aiResponse.Content.ReadAsStringAsync();
                Console.WriteLine("AI RESPONSE: " + raw);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                explain = JsonSerializer.Deserialize<ExplainResult>(raw, options);
            }
            else
            {
                var err = await aiResponse.Content.ReadAsStringAsync();
                Console.WriteLine("AI ERROR: " + err);
            }

            if (explain != null)
            {
                var lastPrediction = await _db.StudentPredictions
                    .Where(p => p.StudentId == student.StudentId)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync();

                if (lastPrediction == null ||
                    Math.Abs(lastPrediction.PredictedScore - explain.predicted_score) > 0.1)
                {
                    var newPrediction = new StudentPrediction
                    {
                        StudentId = student.StudentId,
                        PredictedScore = explain.predicted_score,
                        Level = explain.level
                    };

                    _db.StudentPredictions.Add(newPrediction);
                    await _db.SaveChangesAsync();
                }
            }

            // This will be null if the AI service call fails
            return Ok(new
            {
                studentId,
                studentName = student.FullName,

                metrics = new
                {
                    attendance = latestMetric.AttendancePercentage,
                    studyHours = latestMetric.StudyHours,
                    examScore = lastQuiz?.Score ?? 0
                },


                prediction = explain == null ? null : new
                {
                    predictedScore = explain.predicted_score,
                    level = explain.level,
                    insights = explain.insights ?? new List<Insight>()
                },

                history
            });

        }


        // Endpoint to get the latest prediction for a student
        // FastAPI → Requests .NET → Last Prediction for the student
        [HttpGet("prediction")]
        public async Task<IActionResult> GetPrediction([FromQuery] int studentId)
        {
            var lastPrediction = await _db.StudentPredictions
                .Where(p => p.StudentId == studentId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (lastPrediction == null)
                return NotFound();

            return Ok(new
            {
                predictedScore = lastPrediction.PredictedScore,
                level = lastPrediction.Level
            });
        }
    }
}