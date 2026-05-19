using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolPlatform.Api.Data;
using SchoolPlatform.Api.Models;
using System.Net.Http.Json;

namespace SchoolPlatform.Api.Controllers
{
    [ApiController]
    public class QuizAssignmentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IHttpClientFactory _http;

        public QuizAssignmentsController(AppDbContext db, IHttpClientFactory http)
        {
            _db = db;
            _http = http;
        }


        [HttpPost("api/teacher/{teacherId}/quiz-assignments/create")]
        public async Task<IActionResult> CreateQuizAssignment(int teacherId, [FromBody] CreateQuizAssignmentRequest request)
        {
            var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                return NotFound(new { message = "Teacher not found" });

            var client = _http.CreateClient("AiService");

            var fastApiRequest = new
            {
                topic = request.Topic,
                grade_level = request.GradeLevel,
                subject = request.Subject,
                number_of_questions = request.NumberOfQuestions
            };

            var response = await client.PostAsJsonAsync("/api/quiz/template/generate", fastApiRequest);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "FastAPI quiz generation failed" });

            var template = await response.Content.ReadFromJsonAsync<QuizTemplateResponse>();
            if (template == null)
                return BadRequest(new { message = "Invalid response from FastAPI" });

            var quizAssignment = new QuizAssignment
            {
                TeacherId = teacherId,
                Topic = request.Topic,
                GradeLevel = request.GradeLevel,
                Subject = request.Subject,
                NumberOfQuestions = request.NumberOfQuestions,
                FastApiTemplateId = template.TemplateId,
                CreatedAt = DateTime.UtcNow
            };

            _db.QuizAssignments.Add(quizAssignment);
            await _db.SaveChangesAsync();

            var studentAssignments = request.StudentIds.Select(studentId => new StudentQuizAssignment
            {
                QuizAssignmentId = quizAssignment.QuizAssignmentId,
                StudentId = studentId,
                IsSubmitted = false
            }).ToList();

            _db.StudentQuizAssignments.AddRange(studentAssignments);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Quiz assignment created successfully",
                assignmentId = quizAssignment.QuizAssignmentId,
                templateId = quizAssignment.FastApiTemplateId,
                assignedStudents = studentAssignments.Count
            });
        }

        [HttpGet("api/student/{studentId}/quiz-assignments")]
        public async Task<IActionResult> GetStudentQuizAssignments(int studentId)
        {
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            var assignments = await _db.StudentQuizAssignments
                .Where(sqa => sqa.StudentId == studentId)
                .Join(_db.QuizAssignments,
                      sqa => sqa.QuizAssignmentId,
                      qa => qa.QuizAssignmentId,
                      (sqa, qa) => new
                      {
                          sqa.StudentQuizAssignmentId,
                          sqa.QuizAssignmentId,
                          qa.Topic,
                          qa.Subject,
                          qa.GradeLevel,
                          qa.NumberOfQuestions,
                          qa.CreatedAt,
                          sqa.IsSubmitted,
                          sqa.Score,
                          sqa.MaxScore,
                          sqa.SubmittedAt
                      })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(assignments);
        }

        [HttpGet("api/student/quiz-assignments/{studentQuizAssignmentId}")]
        public async Task<IActionResult> GetStudentQuizAssignmentDetails(int studentQuizAssignmentId)
        {
            var studentAssignment = await _db.StudentQuizAssignments
                .FirstOrDefaultAsync(x => x.StudentQuizAssignmentId == studentQuizAssignmentId);

            if (studentAssignment == null)
                return NotFound(new { message = "Student quiz assignment not found" });

            var quizAssignment = await _db.QuizAssignments
                .FirstOrDefaultAsync(x => x.QuizAssignmentId == studentAssignment.QuizAssignmentId);

            if (quizAssignment == null)
                return NotFound(new { message = "Quiz assignment not found" });

            var client = _http.CreateClient("AiService");
            var response = await client.GetAsync($"/api/quiz/template/{quizAssignment.FastApiTemplateId}");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "Failed to fetch quiz template from FastAPI" });

            var template = await response.Content.ReadFromJsonAsync<QuizTemplateResponse>();
            if (template == null)
                return BadRequest(new { message = "Invalid template response from FastAPI" });

            return Ok(new
            {
                studentQuizAssignmentId = studentAssignment.StudentQuizAssignmentId,
                quizAssignmentId = quizAssignment.QuizAssignmentId,
                topic = quizAssignment.Topic,
                subject = quizAssignment.Subject,
                gradeLevel = quizAssignment.GradeLevel,
                numberOfQuestions = quizAssignment.NumberOfQuestions,
                isSubmitted = studentAssignment.IsSubmitted,
                score = studentAssignment.Score,
                maxScore = studentAssignment.MaxScore,
                submittedAt = studentAssignment.SubmittedAt,
                questions = template.Questions
            });
        }

        [HttpPost("api/student/quiz-assignments/{studentQuizAssignmentId}/submit")]
        public async Task<IActionResult> SubmitStudentQuizAssignment(
        int studentQuizAssignmentId,
        [FromBody] SubmitQuizAnswersRequest request)
        {
            var studentAssignment = await _db.StudentQuizAssignments
                .FirstOrDefaultAsync(x => x.StudentQuizAssignmentId == studentQuizAssignmentId);

            if (studentAssignment == null)
                return NotFound(new { message = "Student quiz assignment not found" });

            if (studentAssignment.IsSubmitted)
                return BadRequest(new { message = "Quiz already submitted" });

            var quizAssignment = await _db.QuizAssignments
                .FirstOrDefaultAsync(x => x.QuizAssignmentId == studentAssignment.QuizAssignmentId);

            if (quizAssignment == null)
                return NotFound(new { message = "Quiz assignment not found" });

            var client = _http.CreateClient("AiService");

            var fastApiRequest = new
            {
                answers = request.Answers.Select(a => new
                {
                    question_id = a.QuestionId,
                    selected_index = a.SelectedIndex
                }).ToList()
            };

            var response = await client.PostAsJsonAsync(
                $"/api/quiz/template/{quizAssignment.FastApiTemplateId}/submit",
                fastApiRequest
            );

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, new { message = "Failed to submit quiz to FastAPI" });

            var result = await response.Content.ReadFromJsonAsync<QuizSubmitResponse>();
            if (result == null)
                return BadRequest(new { message = "Invalid submit response from FastAPI" });

            studentAssignment.IsSubmitted = true;
            studentAssignment.Score = result.Score;
            studentAssignment.MaxScore = result.MaxScore;
            studentAssignment.SubmittedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var latestMetric = await _db.StudentMetrics
                .Where(m => m.StudentId == studentAssignment.StudentId)
                .OrderByDescending(m => m.RecordedAt)
                .FirstOrDefaultAsync();

            _db.StudentMetrics.Add(new StudentMetric
            {
                StudentId = studentAssignment.StudentId,
                StudyHours = latestMetric?.StudyHours ?? 0,
                AttendancePercentage = latestMetric?.AttendancePercentage ?? 0,
                ExamScore = result.Score,
                RecordedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Quiz submitted successfully",
                score = studentAssignment.Score,
                maxScore = studentAssignment.MaxScore,
                submittedAt = studentAssignment.SubmittedAt
            });
        }
    }
}