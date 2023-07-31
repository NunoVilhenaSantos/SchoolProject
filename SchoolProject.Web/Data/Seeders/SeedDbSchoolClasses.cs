using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Seeders.CoursesLists;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbSchoolClasses
{
    private static Random _random;
    private static IUserHelper _userHelper;
    private static ILogger<SeedDbPersons> _logger;
    private static DataContextMsSql _dataContextMssql;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper, ILogger<SeedDbPersons> logger,
        DataContextMsSql dataContextMssql
    )
    {
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMssql = dataContextMssql;
    }


    public static async Task AddingData()
    {
        var user =
            await _userHelper.GetUserByEmailAsync(
                "nuno.santos.26288@formandos.cinel.pt") ??
            new User
            {
                FirstName = "Nuno",
                LastName = "Santos",
                WasDeleted = false
            };

        Console.WriteLine(
            "Seeding courses and school-classes tables with the courses...");


        var coursesForTpsi = CreateCoursesListFromDictionary(
            SeedDbCoursesList.TeAig(), user);


        // Create a list of courses
        var schoolClasses = new List<SchoolClass>
        {
            new()
            {
                Acronym = "TIGR.D.L.00",
                Name = "Técnico/a de Informática - " +
                       "Instalação e Gestão de Redes",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForTigr,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.TPSI.D.L.00",
                Name = "Técnico/a Especialista em Tecnologias e " +
                       "Programação de Sistemas de Informação - TPSI",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.SITE.DPO.95",
                Name = "Técnico/a Especialista em Desenvolvimento de " +
                       "Produtos Multimédia (Laboral) (B-Learning)",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForTdpm,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.GRSI.D.L.00",
                Name = "Técnico/a Especialista em " +
                       "Gestão de Redes e Sistemas Informáticos",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForGrsi,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.ARCI.N.L.00",
                Name = "Tecnico/a Especialista em Automação, " +
                       "Robótica e Controlo Industrial",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForArci,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.DPM.D.L.00",
                Name = "Técnico/a Especialista em " +
                       "Desenvolvimento de Produtos Multimédia",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForTdpm,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.TR.D.L.00",
                Name = "Técnico/a Especialista em Telecomunicações e Redes",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesForTetr,
                Courses = coursesForTpsi
            },
            new()
            {
                Acronym = "CET.SITE.DPO.91",
                Name = "Técnico/a Especialista em Tecnologias e Programação " +
                       "de Sistemas de Informação (Laboral) (B-Learning)",
                StartDate = DateTime.Today.AddMonths(1),
                EndDate = DateTime.Today.AddMonths(13),
                StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
                EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                // Courses = coursesTpsi,
                Courses = coursesForTpsi
            }
            // Add more courses here if needed
        };


        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClasses);

        // Add the courses to the database
        await _dataContextMssql.SaveChangesAsync();


        Console.WriteLine("Seeding completed.");
    }


    private static List<Course> CreateCoursesListFromDictionary(
        Dictionary<string, (string, int, double)> courseDictionary, User user)
    {
        return courseDictionary
            .Select(courseEntry => new Course
            {
                Codigo = courseEntry.Key,
                Name = courseEntry.Value.Item1,
                WorkLoad = courseEntry.Value.Item2,
                Credits = courseEntry.Value.Item3,
                IdGuid = default,
                CreatedBy = user
            })
            .ToList();
    }
}