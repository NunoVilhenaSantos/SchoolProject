using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using SchoolProject.Web.Data.DataContexts.MySQL;

namespace SchoolProject.Web.Data.EntitiesOthers;

/// <summary>
///     Salvar os dados em CSV
/// </summary>
public static class SaveToCsv
{
    // Set the base path here
    private const string FilePath = ".\\Data\\Csv\\";
    private static readonly DataContextMySql DataContext;


    private static readonly object FileLock = new();


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
            .Include(c => c.Country)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .ToList();

        SaveEntitiesToCsv(cities, "Cities.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.Countries.ToList(),
            "Countries.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.Nationalities.ToList(),
            "Nationalities.csv", csvConfig);


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
        SaveEntitiesToCsv(dataContext.CourseDisciplines,
            "CourseDisciplines.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.CourseStudents,
            "CourseStudents.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Disciplines,
            "Disciplines.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Enrollments,
            "Enrollments.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Students,
            "Students.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.StudentDisciplines,
            "StudentDisciplines.csv", csvConfig);


        SaveEntitiesToCsv(dataContext.Teachers,
            "Teachers.csv", csvConfig);
        SaveEntitiesToCsv(dataContext.TeacherDisciplines,
            "TeacherDisciplines.csv", csvConfig);
    }


    private static void SaveEntitiesToCsvOld<T>(IEnumerable<T> entities,
        string fileName, CsvConfiguration csvConfig)
    {
        Directory.CreateDirectory(FilePath);

        var filePath = Path.Combine(FilePath, fileName);

        lock (FileLock)
        {
            using (var writer = new StreamWriter(filePath, false,
                       Encoding.UTF8))
            {
                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    var tableTemp = entities.ToList();
                    csv.WriteRecords(tableTemp);
                }
            }
        }
    }


    private static void SaveEntitiesToCsv<T>(IEnumerable<T> entities,
        string fileName, CsvConfiguration csvConfig)
    {
        Directory.CreateDirectory(FilePath);

        var filePath = Path.Combine(FilePath, fileName);

        var fileAvailable = false;


        while (!fileAvailable)
            try
            {
                // Tenta abrir o arquivo para verificar se ele está disponível.
                using (var fileStream =
                       File.Open(filePath, FileMode.Open,
                           FileAccess.Read, FileShare.None))
                {
                    // Se conseguir abrir, o arquivo está disponível.
                    fileAvailable = true;
                }
            }
            catch (IOException)
            {
                // Se ocorrer uma exceção, o arquivo está em uso.
                // Aguarde um pouco e tente novamente.
                // Aguarda por 1 segundo antes de verificar novamente.
                Thread.Sleep(1000);
            }


        // Agora que o arquivo não está mais em uso,
        // você pode escrever nele com segurança.
        using (var writer =
               new StreamWriter(filePath, false, Encoding.UTF8))
        {
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                var tableTemp = entities.ToList();
                csv.WriteRecords(tableTemp);
            }
        }
    }
}