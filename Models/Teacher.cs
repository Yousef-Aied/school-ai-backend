namespace SchoolPlatform.Api.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";

        public ICollection<TeacherStudent> TeacherStudents { get; set; } = new List<TeacherStudent>();
    }
}
