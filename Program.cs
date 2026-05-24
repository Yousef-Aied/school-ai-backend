using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolPlatform.Api.Data;
using SchoolPlatform.Api.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// HttpClient Factory
builder.Services.AddHttpClient();

// EF Core 
builder.Services.AddDbContext<AppDbContext>(options =>
         options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

//options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


// JWT Auth
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

builder.Services.AddAuthorization();

// HttpClient
builder.Services.AddHttpClient("AiService", client =>
{
    //client.BaseAddress = new Uri("http://127.0.0.1:8000"); // FastAPI Local
    client.BaseAddress = new Uri("https://school-ai-rag-production.up.railway.app"); // FastAPI Render
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//.WithOrigins("http://localhost:5173")
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseCors();
//app.UseCors("AllowVite");

//app.UseCors("AllowAll"); 

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://0.0.0.0:{port}");




using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Students.Any())
    {
        db.Students.AddRange(

            new Student
            {
                FullName = "Ahmed Ali",
                GradeLevel = 10,
                Age = 13,
                Gender = "Male",
                SchoolType = "Public",
                InternetAccess = "Yes",
                ExtraActivities = "No",
                StudyMethod = "Self",
                TravelTime = 10,

                Metrics = new List<StudentMetric>
                {
                    new StudentMetric
                    {
                        StudyHours = 5,
                        AttendancePercentage = 90,
                        ExamScore = 85
                    }
                }
            },

            new Student
            {
                FullName = "Sara Mohamed",
                GradeLevel = 8,
                Age = 11,
                Gender = "Female",
                SchoolType = "Private",
                InternetAccess = "Yes",
                ExtraActivities = "Yes",
                StudyMethod = "Group",
                TravelTime = 15,

                Metrics = new List<StudentMetric>
                {
                    new StudentMetric
                    {
                        StudyHours = 7,
                        AttendancePercentage = 95,
                        ExamScore = 92
                    }
                }
            },

            new Student
            {
                FullName = "Omar Hassan",
                GradeLevel = 12,
                Age = 15,
                Gender = "Male",
                SchoolType = "Public",
                InternetAccess = "No",
                ExtraActivities = "Yes",
                StudyMethod = "Online",
                TravelTime = 5,

                Metrics = new List<StudentMetric>
                {
                    new StudentMetric
                    {
                        StudyHours = 3,
                        AttendancePercentage = 70,
                        ExamScore = 60
                    }
                }
            }
        );

        db.SaveChanges();
    }


    if (!db.Teachers.Any())
    {
        var teacher = new Teacher
        {
            FullName = "Test Teacher",
            Email = "teacher@test.com"
        };

        db.Teachers.Add(teacher);
        db.SaveChanges();

        var students = db.Students.ToList();

        foreach (var student in students)
        {
            db.TeacherStudents.Add(new TeacherStudent
            {
                TeacherId = teacher.TeacherId,
                StudentId = student.StudentId
            });
        }

        db.SaveChanges();
    }
}

app.Run();