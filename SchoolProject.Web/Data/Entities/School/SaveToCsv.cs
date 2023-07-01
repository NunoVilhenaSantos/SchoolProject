using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SchoolProject.Web.Data.Entities.School;

public static class SaveToCsv
{
    public static void SaveTo(string filePath)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, csvConfig))
        {
            // Write the courses to the CSV file
            csv.WriteRecords(Courses.Courses.CoursesList);

            // Write the school classes to the CSV file
            csv.WriteRecords(SchoolClasses.SchoolClasses.SchoolClassesList);

            // Write the students to the CSV file
            csv.WriteRecords(Students.Students.StudentsList);

            // Write the teachers to the CSV file
            csv.WriteRecords(Teachers.Teachers.TeachersList);

            // Write the school class courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseClasses)
            foreach (var courseId in kvp.Value)
            {
                csv.WriteRecord(new
                {
                    SchoolClassId = kvp.Key,
                    CourseId = courseId
                });
                csv.NextRecord();
            }

            // Write the student courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseStudents)
            foreach (var courseId in kvp.Value)
            {
                csv.WriteRecord(new
                {
                    StudentId = kvp.Key,
                    CourseId = courseId
                });
                csv.NextRecord();
            }

            // Write the teacher courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseTeacher)
            foreach (var courseId in kvp.Value)
            {
                csv.WriteRecord(new
                {
                    TeacherId = kvp.Key,
                    CourseId = courseId
                });
                csv.NextRecord();
            }
        }
    }
}