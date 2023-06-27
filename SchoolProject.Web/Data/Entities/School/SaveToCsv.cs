using CsvHelper;
using CsvHelper.Configuration;
using System.IO;


namespace ClassLibrary.School;

public static class SaveToCsv
{
    public static void SaveTo(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration()))
        {
            // Write the courses to the CSV file
            csv.WriteRecords(_courses.Values);

            // Write the school classes to the CSV file
            csv.WriteRecords(_schoolClasses.Values);

            // Write the students to the CSV file
            csv.WriteRecords(_students.Values);

            // Write the teachers to the CSV file
            csv.WriteRecords(_teachers.Values);

            // Write the school class courses to the CSV file
            foreach (var kvp in _schoolClassCourses)
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
            foreach (var kvp in _studentCourses)
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
            foreach (var kvp in _teacherCourses)
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