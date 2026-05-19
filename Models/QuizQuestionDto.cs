using System.Text.Json.Serialization;

namespace SchoolPlatform.Api.Models
{
    public class QuizQuestionDto
    {
        [JsonPropertyName("question_id")]
        public string QuestionId { get; set; } = "";

        [JsonPropertyName("question_text")]
        public string QuestionText { get; set; } = "";

        [JsonPropertyName("choices")]
        public List<string> Choices { get; set; } = new();
    }
}