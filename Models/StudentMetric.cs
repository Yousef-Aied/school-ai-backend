using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolPlatform.Api.Models
{
    public class StudentMetric
    {
        [Key]
        public int MetricId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; } = default!;

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        public double StudyHours { get; set; }
        public double AttendancePercentage { get; set; }

        public double ExamScore { get; set; }
    }
}