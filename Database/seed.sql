-- Seed Data for Students & Metrics
-- Used for AI Prediction Testing

DELETE FROM StudentMetrics;
DELETE FROM StudentPredictions;
DELETE FROM Students;

DBCC CHECKIDENT ('Students', RESEED, 0);
DBCC CHECKIDENT ('StudentMetrics', RESEED, 0);
DBCC CHECKIDENT ('StudentPredictions', RESEED, 0);

INSERT INTO Students 
(FullName, GradeLevel, Age, Gender, SchoolType, InternetAccess, ExtraActivities, StudyMethod, TravelTime)
VALUES
('Zaid Ahmed', 10, 16, 'Male', 'Public', 'Yes', 'Yes', 'SelfStudy', 30),
('Sara Ali', 9, 15, 'Female', 'Private', 'Yes', 'No', 'GroupStudy', 15),
('Omar Khaled', 11, 17, 'Male', 'Public', 'No', 'Yes', 'OnlineVideos', 45);

SELECT StudentId, FullName FROM Students;

INSERT INTO StudentMetrics 
(StudentId, StudyHours, AttendancePercentage, RecordedAt)
VALUES
(1, 5, 85, GETDATE()),
(1, 6, 88, GETDATE()),
(1, 7, 90, GETDATE()),

(2, 3, 70, GETDATE()),
(2, 4, 75, GETDATE()),

(3, 2, 60, GETDATE()),
(3, 3, 65, GETDATE());