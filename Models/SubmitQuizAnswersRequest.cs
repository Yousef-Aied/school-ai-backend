namespace SchoolPlatform.Api.Models
{
    public class SubmitQuizAnswersRequest
    {
        public List<QuizAnswerItem> Answers { get; set; } = new();
    }

    public class QuizAnswerItem
    {
        public string QuestionId { get; set; } = "";
        public int SelectedIndex { get; set; }
    }
}