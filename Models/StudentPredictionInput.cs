namespace SchoolPlatform.Api.Models
{
    public class StudentPredictionInput
    {
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public string SchoolType { get; set; } = "";
        public double StudyHours { get; set; }
        public double AttendancePercentage { get; set; }
        public string InternetAccess { get; set; } = "";
        public double TravelTime { get; set; }
        public string ExtraActivities { get; set; } = "";
        public string StudyMethod { get; set; } = "";
        // 'overall_score'  What we expect

    }
}
