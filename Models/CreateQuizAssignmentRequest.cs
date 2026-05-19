namespace SchoolPlatform.Api.Models
{
    public class CreateQuizAssignmentRequest
    {
        public string Topic { get; set; } = "";
        public int GradeLevel { get; set; }
        public string Subject { get; set; } = "";
        public int NumberOfQuestions { get; set; }
        public List<int> StudentIds { get; set; } = new();
    }
}
