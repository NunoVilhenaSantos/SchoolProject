using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Seeders.DisciplinesLists;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
/// </summary>
public class SeedDbCourses
{
    // private static Random _random;

    // private static IUserHelper _userHelper;
    // private static ILogger<SeedDbSchoolClasses> _logger;

    private static DataContextMySql _dataContextInUse;
    // private static DataContextMsSql _dataContextMsSql;

    // Add a private static field to store the existing courses
    private static List<Discipline> _listOfDisciplinesFromDb;
    private static List<Course> _listOfCoursesFromDb;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        DataContextMySql dataContextInUse
    )
    {
        _dataContextInUse = dataContextInUse;
    }


    public static async Task AddingData(AppUser appUser)
    {
        Console.WriteLine(
            "Seeding courses and school-classes tables with the courses...");

        SeedDBDisciplinesLists.Initialize(_dataContextInUse);

        await SeedDBDisciplinesLists.AddingData(appUser);

        // ------------------------------------------------------------------ //

        // Get the courses from the database
        _listOfDisciplinesFromDb =
            await _dataContextInUse.Disciplines.ToListAsync();

        // Get the school-classes from the database
        _listOfCoursesFromDb =
            await _dataContextInUse.Courses.ToListAsync();

        Console.WriteLine("debug zone...");

        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        //
        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        var forTeGrsi = SeedDBDisciplinesLists.TeGrsi();

        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        var forTeAig = SeedDBDisciplinesLists.TeAig();

        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        var forTeTpsi = SeedDBDisciplinesLists.TeTpsi();

        // 481344
        // Técnico/a Especialista em Cibersegurança
        var forTeCs = SeedDBDisciplinesLists.TeCs();

        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        var forTeGicd = SeedDBDisciplinesLists.TeGicd();

        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        var forTeArci = SeedDBDisciplinesLists.TeArci();

        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        var forTeDpm = SeedDBDisciplinesLists.TeDpm();

        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        var forTeTr = SeedDBDisciplinesLists.TeTr();

        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        //
        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        var listForTeGrsi =
            GetExistingDisciplinesAsync(forTeGrsi).Result;

        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        var listForTeAig =
            GetExistingDisciplinesAsync(forTeAig).Result;

        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        var listForTeTpsi =
            GetExistingDisciplinesAsync(forTeTpsi).Result;

        // 481344
        // Técnico/a Especialista em Cibersegurança
        var listForTeCs =
            GetExistingDisciplinesAsync(forTeCs).Result;

        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        var listForTeGicd =
            GetExistingDisciplinesAsync(forTeGicd).Result;

        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        var listForTeArci =
            GetExistingDisciplinesAsync(forTeArci).Result;

        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        var listForTeDpm =
            GetExistingDisciplinesAsync(forTeDpm).Result;

        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        var listForTeTr =
            GetExistingDisciplinesAsync(forTeTr).Result;

        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        //
        await SeedCourseIfNotExists("481228", "CET.GRSI.D.L.00",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeGrsi, appUser);

        await SeedCourseIfNotExists("481228", "CET.SITE.DPO.92",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeGrsi, appUser);

        await SeedCourseIfNotExists("481228", "CET.SITE.DPO.06",
            "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            listForTeGrsi, appUser);


        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        await SeedCourseIfNotExists("481227", "CET.TEAIG.D.L.00",
            "Técnico/a Especialista em Aplicações Informáticas de Gestão",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeAig, appUser);


        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        await SeedCourseIfNotExists("481241", "CET.TPSI.D.L.00",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeTpsi, appUser);

        await SeedCourseIfNotExists("481241", "CET.TPSI.N.L.00",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-laboral)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            listForTeTpsi, appUser);

        await SeedCourseIfNotExists("481241", "CET.SITE.DPO.91",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeTpsi, appUser);

        await SeedCourseIfNotExists("481241", "CET.SITE.DPO.04",
            "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            listForTeTpsi, appUser);


        // 481344
        // Técnico/a Especialista em Cibersegurança
        await SeedCourseIfNotExists("481241", "CET.SITE.DPO.99",
            "Técnico/a Especialista em Cibersegurança (Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeCs, appUser);
        await SeedCourseIfNotExists("481241", "CET.SITE.DPO.98",
            "Técnico/a Especialista em Cibersegurança (Pós-Laboral) (B-Learning)",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            listForTeCs, appUser);


        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        await SeedCourseIfNotExists("481390", "CET.SITE.99",
            "Técnico/a Especialista em Gestão de Informação e Ciência dos Dados",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeGicd, appUser);


        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        await SeedCourseIfNotExists("523229", "CET.ARCI.N.L.00",
            "Técnico/a Especialista em Automação, Robótica e Controlo Industrial",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeArci, appUser);


        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        await SeedCourseIfNotExists("213240", "CET.ARCI.N.L.00",
            "Técnico/a Especialista em Desenvolvimento de Produtos Multimédia",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(TimeSpan.FromHours(19)),
            TimeSpan.Zero.Add(TimeSpan.FromHours(23)),
            listForTeDpm, appUser);


        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        await SeedCourseIfNotExists("523273", "CET.TR.D.L.00",
            "Técnico/a Especialista em Telecomunicações e Redes",
            5, 5,
            DateTime.Today.AddMonths(1),
            DateTime.Today.AddMonths(13),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(30))),
            TimeSpan.Zero.Add(
                TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(30))),
            listForTeTr, appUser);


        // Add other school classes and courses here...

        // Add more courses here if needed

        Console.WriteLine("Seeding completed.");
    }


    private static Task<HashSet<Discipline>> GetExistingDisciplinesAsync(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        // Filter out the disciplines that are already in the database and also present in the dictionary
        var existingDisciplines = _listOfDisciplinesFromDb
            .Where(d => mergedDictionary.ContainsKey(d.Code))
            .ToHashSet();

        // Get the list of existing courses as a list
        var existingDisciplinesList = existingDisciplines.ToHashSet();

        // return the list of existing courses as a list
        return Task.FromResult(existingDisciplinesList);
    }


    private static async Task SeedCourseIfNotExists(
        string code, string acronym, string name, byte qnqLevel, byte eqfLevel,
        DateTime startDate, DateTime endDate,
        TimeSpan startHour, TimeSpan endHour,
        HashSet<Discipline> disciplines,
        AppUser appUser)
    {
        // Get the course from the database
        var existingCourse = _listOfCoursesFromDb.FirstOrDefault(
            s => s.Acronym == acronym && s.Code == code);


        // Calculate the total duration in hours from the sum of course hours
        var totalDurationInHours = disciplines.Sum(c => c.Hours);


        // Calculate the total number of days needed for completion
        var totalDays =
            (int) Math.Ceiling(totalDurationInHours /
                               ((endHour - startHour).TotalHours - 1));


        // Calculate the end date and time based on the total number of days and start date/hour
        endDate = startDate.AddDays(totalDays - 1).Date.Add(endHour);
        var durationPerDay = endHour - startHour;
        if (endDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            // Adjust the end date if it falls on a weekend
            var daysToAdd = endDate.DayOfWeek == DayOfWeek.Saturday ? 2 : 1;
            endDate = endDate.AddDays(daysToAdd).Date
                .Add(startHour + durationPerDay);
        }


        // Check if the course already exists in the database
        if (existingCourse == null)
        {
            var course = new Course
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
                CreatedBy = appUser,
                ProfilePhotoId = default,

                CourseDisciplines = new HashSet<CourseDiscipline>(),
                CourseStudents = new HashSet<CourseStudent>(),
                Enrollments = new HashSet<Enrollment>(),
            };


            var courseDisciplines = new HashSet<CourseDiscipline>();
            foreach (var discipline in disciplines)
            {
                courseDisciplines.Add(new CourseDiscipline
                    {
                        CourseId = course.Id,
                        Course = course,
                        DisciplineId = discipline.Id,
                        Discipline = discipline,
                        CreatedById = appUser.Id,
                        CreatedBy = appUser,
                    }
                );
            }

            course.CourseDisciplines = courseDisciplines;


            Console.WriteLine("debug zone...");


            await _dataContextInUse.Courses.AddAsync(course);
            await _dataContextInUse.SaveChangesAsync();
        }


        Console.WriteLine("debug zone...");
    }
}