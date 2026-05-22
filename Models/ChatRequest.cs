namespace SchoolPlatform.Api.Models
{
    public class ChatRequest
    {
        public string ConversationId { get; set; } = "";
        public string Message { get; set; } = "";
        public int StudentId { get; set; }
        public string Subject { get; set; } = "";
    }
}
