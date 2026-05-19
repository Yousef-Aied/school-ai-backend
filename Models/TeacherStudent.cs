namespace SchoolPlatform.Api.Models
{
    public class TeacherStudent
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = default!;
    }
}
