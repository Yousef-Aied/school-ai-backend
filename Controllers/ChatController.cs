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
        private readonly HttpClient _aiClient;
        private readonly AppDbContext _db;

        public ChatController(IHttpClientFactory factory, AppDbContext db)
        {
            _aiClient = factory.CreateClient("AiService");
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest req)
        {
            try
            {
                Console.WriteLine("=== CHAT REQUEST ===");

                var student = await _db.Students
                    .FirstOrDefaultAsync(s => s.StudentId == req.StudentId);

                if (student == null)
                {
                    Console.WriteLine("Student not found");
                    return NotFound("Student not found");
                }

                var prediction = await GetPrediction(req.StudentId);

                Console.WriteLine($"Prediction: {prediction?.Level}");

                var aiResponse = await _aiClient.PostAsJsonAsync(
                    "/api/chat",
                    new
                    {
                        conversation_id = req.ConversationId,
                        message = req.Message,
                        student_id = req.StudentId,
                        student_name = student.FullName,
                        grade = student.GradeLevel,
                        subject = req.Subject,
                        student_level = prediction?.Level,
                        predicted_score = prediction?.PredictedScore
                    }
                );

                Console.WriteLine($"AI STATUS: {aiResponse.StatusCode}");

                var content = await aiResponse.Content.ReadAsStringAsync();

                Console.WriteLine("AI RESPONSE:");
                Console.WriteLine(content);

                return StatusCode((int)aiResponse.StatusCode, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CHAT ERROR:");
                Console.WriteLine(ex);

                return StatusCode(500, ex.Message);
            }
        }

        private async Task<PredictionResult> GetPrediction(int studentId)
        {
            try
            {
                var res = await _aiClient.GetAsync(
                    $"https://school-ai-backend-2qd1.onrender.com/api/student/prediction?studentId={studentId}"
                );

                if (!res.IsSuccessStatusCode)
                {
                    return new PredictionResult
                    {
                        Level = "Medium",
                        PredictedScore = 70
                    };
                }

                var json = await res.Content.ReadAsStringAsync(); 

                return JsonSerializer.Deserialize<PredictionResult>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                ) ?? new PredictionResult
                {
                    Level = "Medium",
                    PredictedScore = 70
                };
            }
            catch
            {
                return new PredictionResult
                {
                    Level = "Medium",
                    PredictedScore = 70
                };
            }
        }
    }
}