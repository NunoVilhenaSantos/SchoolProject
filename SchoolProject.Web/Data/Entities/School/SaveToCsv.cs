using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.DataContexts;

namespace SchoolProject.Web.Data.Entities.School
{
    public static class SaveToCsv
    {
        // Set the base path here
        static string _filePath = "Data Source=.\\Data\\";

        public static void SaveTo(DataContextMsSql dataContext)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };


            SaveEntitiesToCsv(
                dataContext.Cities,
                "Cities.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.Countries,
                "Countries.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.Nationalities,
                "Nationalities.csv", csvConfig);


            SaveEntitiesToCsv(
                dataContext.Genres,
                "Genres.csv", csvConfig);


            SaveEntitiesToCsv(
                dataContext.Users,
                "Users.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.UserClaims,
                "UserClaims.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.UserLogins,
                "UserLogins.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.UserRoles,
                "UserRoles.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.UserTokens,
                "UserTokens.csv", csvConfig);


            SaveEntitiesToCsv(
                dataContext.Courses,
                "Courses.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.SchoolClasses,
                "SchoolClasses.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.Students,
                "Students.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.Teachers,
                "Teachers.csv", csvConfig);


            SaveEntitiesToCsv(
                dataContext.Enrollments,
                "Enrollments.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.SchoolClassCourses,
                "SchoolClassCourses.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.StudentCourses,
                "StudentCourses.csv", csvConfig);
            SaveEntitiesToCsv(
                dataContext.TeacherCourses,
                "TeacherCourses.csv", csvConfig);
        }

        private static void SaveEntitiesToCsv<T>(IEnumerable<T> entities,
            string fileName, CsvConfiguration csvConfig)
        {
            string filePath = Path.Combine(_filePath, fileName);
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(entities);
            }
        }
    }
}