using System.ComponentModel.DataAnnotations;

namespace SchoolPlatform.Api.Models
{
    public class StudentPrediction
    {
        [Key]
        public int PredictionId { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; } = default!;

        public double PredictedScore { get; set; }

        public string Level { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}