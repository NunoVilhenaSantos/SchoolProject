using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.School;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;


namespace ClassLibrary.School;

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
            csv.WriteRecords(Courses.CoursesList);

            // Write the school classes to the CSV file
            csv.WriteRecords(SchoolClasses.SchoolClassesList);

            // Write the students to the CSV file
            csv.WriteRecords(Students.StudentsList);

            // Write the teachers to the CSV file
            csv.WriteRecords(Teachers.TeachersList);

            // Write the school class courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseClasses)
            {
                foreach (var courseId in kvp.Value)
                {
                    csv.WriteRecord(new
                    {
                        SchoolClassId = kvp.Key,
                        CourseId = courseId
                    });
                    csv.NextRecord();
                }
            }

            // Write the student courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseStudents)
            {
                foreach (var courseId in kvp.Value)
                {
                    csv.WriteRecord(new
                    {
                        StudentId = kvp.Key,
                        CourseId = courseId
                    });
                    csv.NextRecord();
                }
            }

            // Write the teacher courses to the CSV file
            foreach (var kvp in SchoolDatabase.CourseTeacher)
            {
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
}