using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesMatrix;
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
                {FirstName = "Nuno", LastName = "Santos", WasDeleted = false};


        Console.WriteLine(
            "Seeding courses and school-classes tables with the courses...");


        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        // 5
        var coursesForTeGrsi =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeGrsi(), user);
        var schoolClass = new SchoolClass
        {
            Code = "481228",
            Acronym = "CET.GRSI.D.L.00",
            Name = "Técnico/a Especialista em " +
                   "Gestão de Redes e Sistemas Informáticos ",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeGrsi,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        // 5
        var coursesForTeAig =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeAig(), user);
        schoolClass = new SchoolClass
        {
            Code = "481227",
            Acronym = "CET.TEAIG.D.L.00",
            Name =
                "Técnico/a Especialista em Aplicações Informáticas de Gestão",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeAig,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        // 5
        var coursesForTeTpsi =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeTpsi(), user);
        schoolClass = new SchoolClass
        {
            Code = "481241",
            Acronym = "CET.TPSI.D.L.00",
            Name = "Técnico/a Especialista em Tecnologias e " +
                   "Programação de Sistemas de Informação",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeTpsi
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        schoolClass = new SchoolClass
        {
            Code = "481241",
            Acronym = "CET.SITE.DPO.91",
            Name = "Técnico/a Especialista em Tecnologias e Programação " +
                   "de Sistemas de Informação (Laboral) (B-Learning)",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeTpsi,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        // 481344
        // Técnico/a Especialista em Cibersegurança
        // 5
        var coursesForTeCs =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeCs(), user);
        schoolClass = new SchoolClass
        {
            Code = "481344",
            Acronym = "CET.SITE.DPO.91",
            Name = "Técnico/a Especialista em Tecnologias e Programação " +
                   "de Sistemas de Informação (Laboral) (B-Learning)",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeCs,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        // 5
        var coursesForTeGicd =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeGicd(), user);
        schoolClass = new SchoolClass
        {
            Code = "481390",
            Acronym = "CET.SITE.DPO.95",
            Name = "Técnico/a Especialista em " +
                   "Gestão de Informação e Ciência dos Dados",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeGicd,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        var coursesForTeArci =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeArci(), user);
        schoolClass = new SchoolClass
        {
            Code = "523229",
            Acronym = "CET.ARCI.N.L.00",
            Name = "Tecnico/a Especialista em Automação, " +
                   "Robótica e Controlo Industrial",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeArci,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        var coursesForTeDpm =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeDpm(), user);
        schoolClass = new SchoolClass
        {
            Code = "213240",
            Acronym = "CET.DPM.D.L.00",
            Name = "Técnico/a Especialista em " +
                   "Desenvolvimento de Produtos Multimédia",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeDpm,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        var coursesForTeTr =
            CreateCoursesListFromDictionary(SeedDbCoursesList.TeTr(), user);
        schoolClass = new SchoolClass
        {
            Code = "523273",
            Acronym = "CET.TR.D.L.00",
            Name = "Técnico/a Especialista em Telecomunicações e Redes",
            QnqLevel = 5,
            EqfLevel = 5,
            StartDate = DateTime.Today.AddMonths(1),
            EndDate = DateTime.Today.AddMonths(13),
            StartHour = TimeSpan.Zero.Add(TimeSpan.FromHours(8)),
            EndHour = TimeSpan.Zero.Add(TimeSpan.FromHours(16)),
            PriceForEmployed = 200,
            PriceForUnemployed = 0,
            IdGuid = default,
            CreatedBy = user,
            Courses = coursesForTeTr,
        };
        await _dataContextMssql.SchoolClasses.AddRangeAsync(schoolClass);
        await _dataContextMssql.SaveChangesAsync();


        // Add more courses here if needed
        // ...

        Console.WriteLine("Seeding completed.");
    }


    private static List<Course> CreateCoursesListFromDictionary(
        Dictionary<string, (string, int, double)> courseDictionary, User user)
    {
        return courseDictionary.Select(courseEntry
                => new Course
                {
                    Code = courseEntry.Key,
                    Name = courseEntry.Value.Item1,
                    WorkLoad = courseEntry.Value.Item2,
                    Credits = courseEntry.Value.Item3,
                    IdGuid = default,
                    CreatedBy = user
                })
            .ToList();
    }
}