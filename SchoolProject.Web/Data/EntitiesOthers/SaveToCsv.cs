using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.DataContexts.MySQL;

namespace SchoolProject.Web.Data.EntitiesOthers;

/// <summary>
/// Salvar os dados em CSV
/// </summary>
public static class SaveToCsv
{
    // Set the base path here
    private const string FilePath = ".\\Data\\Csv\\";
    private static readonly DataContextMySql DataContext;


    /// <summary>
    /// </summary>
    /// <param name="dataContext"></param>
    public static void SaveTo(DataContextMySql dataContext)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };


        Directory.CreateDirectory(FilePath);


            // Certifique-se de que dataContext.Cities contém dados válidos da classe City.
            var cities = dataContext.Cities
            .Include(c =>c.Country)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .ToList();


        SaveEntitiesToCsv(cities, "Cities.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Countries.ToList(),            "Countries.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.Nationalities.ToList(),            "Nationalities.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Genders, "Genders.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Users,
            "Users.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.UserClaims,
            "UserClaims.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.UserLogins,
            "UserLogins.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.UserRoles,
            "UserRoles.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.UserTokens,
            "UserTokens.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Courses,
            "Courses.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.CoursesDisciplines,
            "CourseDisciplines.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.CoursesStudents,
            "CoursesStudents.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Disciplines,
            "Disciplines.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Enrollments,
            "Enrollments.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Students,
            "Students.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.StudentCourses,
            "StudentCourses.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Teachers,
            "Teachers.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.TeacherCourses,
            "TeacherCourses.csv", csvConfig);
    }


    private static void SaveEntitiesToCsv<T>(IEnumerable<T> entities,
        string fileName, CsvConfiguration csvConfig)
    {
        Directory.CreateDirectory(FilePath);

        var filePath = Path.Combine(FilePath, fileName);

        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, csvConfig);

        var tableTemp = entities.ToList();
        csv.WriteRecords(tableTemp);

        // csv.WriteRecords(entities);
    }
}