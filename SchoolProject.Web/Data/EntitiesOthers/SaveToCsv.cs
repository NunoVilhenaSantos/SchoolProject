using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.DataContexts.MSSQL;

namespace SchoolProject.Web.Data.EntitiesOthers;

public static class SaveToCsv
{
    // Set the base path here
    private static readonly string _filePath = "Data Source=.\\Data\\";

    public static void SaveTo(DataContextMsSql dataContext)
    {
        var csvConfig = new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };


        SaveEntitiesToCsv(
            entities: dataContext.Cities,
            fileName: "Cities.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.Countries,
            fileName: "Countries.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.Nationalities,
            fileName: "Nationalities.csv", csvConfig: csvConfig);


        SaveEntitiesToCsv(
            entities: dataContext.Genders,
            fileName: "Genders.csv", csvConfig: csvConfig);


        SaveEntitiesToCsv(
            entities: dataContext.Users,
            fileName: "Users.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.UserClaims,
            fileName: "UserClaims.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.UserLogins,
            fileName: "UserLogins.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.UserRoles,
            fileName: "UserRoles.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.UserTokens,
            fileName: "UserTokens.csv", csvConfig: csvConfig);


        SaveEntitiesToCsv(
            entities: dataContext.Courses,
            fileName: "Courses.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.SchoolClasses,
            fileName: "SchoolClasses.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.Students,
            fileName: "Students.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.Teachers,
            fileName: "Teachers.csv", csvConfig: csvConfig);


        SaveEntitiesToCsv(
            entities: dataContext.Enrollments,
            fileName: "Enrollments.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.SchoolClassCourses,
            fileName: "SchoolClassCourses.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.StudentCourses,
            fileName: "StudentCourses.csv", csvConfig: csvConfig);
        SaveEntitiesToCsv(
            entities: dataContext.TeacherCourses,
            fileName: "TeacherCourses.csv", csvConfig: csvConfig);
    }

    private static void SaveEntitiesToCsv<T>(IEnumerable<T> entities,
        string fileName, CsvConfiguration csvConfig)
    {
        var filePath = Path.Combine(path1: _filePath, path2: fileName);
        using (var writer = new StreamWriter(path: filePath))
        using (var csv = new CsvWriter(writer: writer, configuration: csvConfig))
        {
            csv.WriteRecords(records: entities);
        }
    }
}