using Microsoft.EntityFrameworkCore;
using SchoolPlatform.Api.Models;

namespace SchoolPlatform.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudentMetric> StudentMetrics => Set<StudentMetric>();
        public DbSet<StudentPrediction> StudentPredictions => Set<StudentPrediction>();

        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<TeacherStudent> TeacherStudents => Set<TeacherStudent>();

        public DbSet<QuizAssignment> QuizAssignments { get; set; }
        public DbSet<StudentQuizAssignment> StudentQuizAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherStudent>()
                .HasKey(ts => new { ts.TeacherId, ts.StudentId });

            base.OnModelCreating(modelBuilder);
        }
    }
}