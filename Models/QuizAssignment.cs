namespace SchoolPlatform.Api.Models
{
    public class QuizAssignment
    {
        public int QuizAssignmentId { get; set; }
        public int TeacherId { get; set; }
        public string Topic { get; set; } = "";
        public int GradeLevel { get; set; }
        public string Subject { get; set; } = "";
        public int NumberOfQuestions { get; set; }
        public string FastApiTemplateId { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
