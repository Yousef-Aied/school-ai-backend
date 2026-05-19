namespace SchoolPlatform.Api.Models
{
    public class StudentQuizAssignment
    {
        public int StudentQuizAssignmentId { get; set; }
        public int QuizAssignmentId { get; set; }
        public int StudentId { get; set; }

        public bool IsSubmitted { get; set; } = false;
        public int? Score { get; set; }
        public int? MaxScore { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
