namespace SchoolPlatform.Api.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; } = "";
        public int GradeLevel { get; set; }
        public ICollection<StudentPrediction> Predictions { get; set; } = new List<StudentPrediction>();
        public ICollection<StudentMetric> Metrics { get; set; } = new List<StudentMetric>();

        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public string SchoolType { get; set; } = "";
        public string InternetAccess { get; set; } = "";
        public string ExtraActivities { get; set; } = "";
        public string StudyMethod { get; set; } = "";
        public double TravelTime { get; set; }
    }
}
