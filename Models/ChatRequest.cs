using System.Text.Json.Serialization;

namespace SchoolPlatform.Api.Models
{
    public class ChatRequest
    {
        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; } = "";

        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }

        [JsonPropertyName("grade")]
        public string Grade { get; set; } = "";

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = "";
    }
}
