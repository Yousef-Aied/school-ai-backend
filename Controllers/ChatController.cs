using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolPlatform.Api.Data;
using SchoolPlatform.Api.Models;
using System.Text.Json;

namespace SchoolPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _db;

        public ChatController(IHttpClientFactory factory, AppDbContext db)
        {
            _httpClient = factory.CreateClient();
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req)
        {
            // student
            var student = await _db.Students
                .FirstOrDefaultAsync(s => s.StudentId == req.StudentId);

            if (student == null)
                return NotFound("Student not found");

            // prediction
            var prediction = await GetPrediction(req.StudentId);

            // send to AI
            var aiResponse = await _httpClient.PostAsJsonAsync(
                "https://ai-service-pj5r.onrender.com/api/chat",
                new
                {
                    conversation_id = req.ConversationId,
                    message = req.Message,
                    student_id = req.StudentId,
                    student_name = student.FullName,
                    grade = student.GradeLevel,
                    subject = req.Subject,
                    student_level = prediction.Level,
                    predicted_score = prediction.PredictedScore
                }
            );

            var content = await aiResponse.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        private async Task<PredictionResult> GetPrediction(int studentId)
        {
            var res = await _httpClient.GetAsync(
                $"https://school-ai-backend-2qd1.onrender.com/api/student/prediction?studentId={studentId}"
            );

            var json = await res.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PredictionResult>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            )!;
        }
    }
}