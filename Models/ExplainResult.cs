namespace SchoolPlatform.Api.Models
{
    public class ExplainResult
    {
        public double predicted_score { get; set; }
        public string level { get; set; } = "";
        public List<Insight> insights { get; set; } = new();
    }

    public class Insight
    {
        public string title { get; set; } = "";
        public string text { get; set; } = "";
    }
}