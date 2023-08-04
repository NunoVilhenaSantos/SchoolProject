using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Data.Seeders.CoursesLists;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbSchoolClasses
{
    private static Random _random;

    private static IUserHelper _userHelper;
    private static ILogger<SeedDbSchoolClasses> _logger;

    private static DataContextMsSql _dataContextMsSql;

    // Add a private static field to store the existing courses
    private static List<Course> _listOfCoursesFromDb;
    private static List<SchoolClass> _listOfSchoolClassFromDb;


    public SeedDbSchoolClasses(
        IUserHelper userHelper, ILogger<SeedDbSchoolClasses> logger,
        DataContextMsSql dataContextMsSql
    )
    {
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMsSql = dataContextMsSql;
    }


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper,
        DataContextMsSql dataContextMsSql,
        ILogger<SeedDbSchoolClasses> logger
    )
    {
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMsSql = dataContextMsSql;
    }


    public static async Task AddingData(User user)
    {
        Console.WriteLine(
            "Seeding courses and school-classes tables with the courses...");

        SeedDbCoursesList.Initialize(_dataContextMsSql);

        await SeedDbCoursesList.AddingData(user);

        // ------------------------------------------------------------------ //

        // Get the courses from the database
        _listOfCoursesFromDb = await _dataContextMsSql.Courses.ToListAsync();

        // Get the school-classes from the database
        _listOfSchoolClassFromDb =
            await _dataContextMsSql.SchoolClasses.ToListAsync();

        Console.WriteLine("debug zone...");

        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        //
        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        var coursesForTeGrsi = SeedDbCoursesList.TeGrsi();

        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        var coursesForTeAig = SeedDbCoursesList.TeAig();

        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        var coursesForTeTpsi = SeedDbCoursesList.TeTpsi();

        // 481344
        // Técnico/a Especialista em Cibersegurança
        var coursesForTeCs = SeedDbCoursesList.TeCs();

        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        var coursesForTeGicd = SeedDbCoursesList.TeGicd();

        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        var coursesForTeArci = SeedDbCoursesList.TeArci();

        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        var coursesForTeDpm = SeedDbCoursesList.TeDpm();

        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        var coursesForTeTr = SeedDbCoursesList.TeTr();

        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        //
        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        var coursesListForTeGrsi =
            GetExistingCoursesAsync(coursesForTeGrsi).Result;

        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        var coursesListForTeAig =
            GetExistingCoursesAsync(coursesForTeAig).Result;

        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        var coursesListForTeTpsi =
            GetExistingCoursesAsync(coursesForTeTpsi).Result;

        // 481344
        // Técnico/a Especialista em Cibersegurança
        var coursesListForTeCs = GetExistingCoursesAsync(coursesForTeCs).Result;

        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        var coursesListForTeGicd =
            GetExistingCoursesAsync(coursesForTeGicd).Result;

        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        var coursesListForTeArci =
            GetExistingCoursesAsync(coursesForTeArci).Result;

        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        var coursesListForTeDpm =
            GetExistingCoursesAsync(coursesForTeDpm).Result;

        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        var coursesListForTeTr = GetExistingCoursesAsync(coursesForTeTr).Result;

        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        //
        await SeedSchoolClassIfNotExists("481228", "CET.GRSI.D.L.00",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeGrsi, user);

        await SeedSchoolClassIfNotExists("481228", "CET.SITE.DPO.92",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeGrsi, user);

        await SeedSchoolClassIfNotExists("481228", "CET.SITE.DPO.06",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            coursesListForTeGrsi, user);


        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        await SeedSchoolClassIfNotExists("481227", "CET.TEAIG.D.L.00",
            "Técnico/a Especialista em Aplicações Informáticas de Gestão",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeAig, user);


        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        await SeedSchoolClassIfNotExists("481241", "CET.TPSI.D.L.00",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeTpsi, user);

        await SeedSchoolClassIfNotExists("481241", "CET.TPSI.N.L.00",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-laboral)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            coursesListForTeTpsi, user);

        await SeedSchoolClassIfNotExists("481241", "CET.SITE.DPO.91",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeTpsi, user);

        await SeedSchoolClassIfNotExists("481241", "CET.SITE.DPO.04",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            coursesListForTeTpsi, user);


        // 481344
        // Técnico/a Especialista em Cibersegurança
        await SeedSchoolClassIfNotExists("481241", "CET.SITE.DPO.99",
            "Técnico/a Especialista em Cibersegurança (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeCs, user);
        await SeedSchoolClassIfNotExists("481241", "CET.SITE.DPO.98",
            "Técnico/a Especialista em Cibersegurança (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            coursesListForTeCs, user);


        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        await SeedSchoolClassIfNotExists("481390", "CET.SITE.99",
            "Técnico/a Especialista em Gestão de Informação e Ciência dos Dados",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeGicd, user);


        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        await SeedSchoolClassIfNotExists("523229", "CET.ARCI.N.L.00",
            "Técnico/a Especialista em Automação, Robótica e Controlo Industrial",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeArci, user);


        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        await SeedSchoolClassIfNotExists("213240", "CET.ARCI.N.L.00",
            "Técnico/a Especialista em Desenvolvimento de Produtos Multimédia",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            coursesListForTeDpm, user);


        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        await SeedSchoolClassIfNotExists("523273", "CET.TR.D.L.00",
            "Técnico/a Especialista em Telecomunicações e Redes",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            coursesListForTeTr, user);


        // Add other school classes and courses here...

        // Add more courses here if needed

        Console.WriteLine("Seeding completed.");
    }


    private static async Task<List<Course>> GetExistingCoursesAsync(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        // Filter out the courses that are already in the database and also present in the dictionary
        var existingCourses = _listOfCoursesFromDb
            .Where(course => mergedDictionary.ContainsKey(course.Code))
            .ToList();


        // Get the list of existing courses as a list
        var existingCoursesList = existingCourses.ToList();


        // return the list of existing courses as a list
        return existingCoursesList;
    }


    private static async Task SeedSchoolClassIfNotExists(
        string code, string acronym, string name, byte qnqLevel, byte eqfLevel,
        DateTime startDate, DateTime endDate,
        TimeSpan startHour, TimeSpan endHour,
        List<Course> courses,
        User user)
    {
        // Get the school classes from the database
        var existingSchoolClass = _listOfSchoolClassFromDb.FirstOrDefault(
            s => s.Acronym == acronym && s.Code == code);


        // Calculate the total duration in hours from the sum of course hours
        var totalDurationInHours = courses.Sum(c => c.Hours);


        // Calculate the total number of days needed for completion
        var totalDays =
            (int) Math.Ceiling(totalDurationInHours /
                               ((endHour - startHour).TotalHours - 1));


        // Calculate the end date and time based on the total number of days and start date/hour
        endDate = startDate.AddDays(totalDays - 1).Date.Add(endHour);
        var durationPerDay = endHour - startHour;
        if (endDate.DayOfWeek == DayOfWeek.Saturday ||
            endDate.DayOfWeek == DayOfWeek.Sunday)
        {
            // Adjust the end date if it falls on a weekend
            var daysToAdd = endDate.DayOfWeek == DayOfWeek.Saturday ? 2 : 1;
            endDate =
                endDate.AddDays(daysToAdd).Date.Add(startHour + durationPerDay);
        }


        // Check if the school class already exists in the database
        if (existingSchoolClass == null)
        {
            var schoolClass = new SchoolClass
            {
                Code = code,
                Acronym = acronym,
                Name = name,
                QnqLevel = qnqLevel,
                EqfLevel = eqfLevel,
                StartDate = startDate,
                EndDate = endDate,
                StartHour = startHour,
                EndHour = endHour,
                PriceForEmployed = 200,
                PriceForUnemployed = 0,
                IdGuid = default,
                CreatedBy = user,
                Courses = courses
            };

            await _dataContextMsSql.SchoolClasses.AddAsync(schoolClass);
            await _dataContextMsSql.SaveChangesAsync();
        }
    }
}