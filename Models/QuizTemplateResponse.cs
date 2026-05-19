using System.Text.Json.Serialization;


namespace SchoolPlatform.Api.Models
{
    public class QuizTemplateResponse
    {
        [JsonPropertyName("template_id")]
        public string TemplateId { get; set; } = "";

        [JsonPropertyName("questions")]
        public List<QuizQuestionDto> Questions { get; set; } = new();
    }
}
