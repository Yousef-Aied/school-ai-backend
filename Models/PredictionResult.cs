using System.Text.Json.Serialization;

namespace SchoolPlatform.Api.Models
{
    public class PredictionResult
    {
        [JsonPropertyName("predicted_score")]
        public double PredictedScore { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; } = "";
    }
}
