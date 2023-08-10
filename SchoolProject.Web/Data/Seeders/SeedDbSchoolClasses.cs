using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Data.Seeders.CoursesLists;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbSchoolClasses
{
    // private static Random _random;

    // private static IUserHelper _userHelper;
    // private static ILogger<SeedDbSchoolClasses> _logger;

    private static DataContextMySql _dataContextInUse;
    // private static DataContextMsSql _dataContextMsSql;

    // Add a private static field to store the existing courses
    private static List<Course> _listOfCoursesFromDb;
    private static List<SchoolClass> _listOfSchoolClassFromDb;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        DataContextMySql dataContextInUse
    )
    {
        _dataContextInUse = dataContextInUse;
    }


    public static async Task AddingData(User user)
    {
        Console.WriteLine(
            value: "Seeding courses and school-classes tables with the courses...");

        SeedDbCoursesList.Initialize(dataContextInUse: _dataContextInUse);

        await SeedDbCoursesList.AddingData(user: user);

        // ------------------------------------------------------------------ //

        // Get the courses from the database
        _listOfCoursesFromDb = await _dataContextInUse.Courses.ToListAsync();

        // Get the school-classes from the database
        _listOfSchoolClassFromDb =
            await _dataContextInUse.SchoolClasses.ToListAsync();

        Console.WriteLine(value: "debug zone...");

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

        Console.WriteLine(value: "debug zone...");


        // ------------------------------------------------------------------ //
        //
        // 481228
        // Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos
        var coursesListForTeGrsi =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeGrsi).Result;

        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        var coursesListForTeAig =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeAig).Result;

        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        var coursesListForTeTpsi =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeTpsi).Result;

        // 481344
        // Técnico/a Especialista em Cibersegurança
        var coursesListForTeCs = GetExistingCoursesAsync(mergedDictionary: coursesForTeCs).Result;

        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        var coursesListForTeGicd =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeGicd).Result;

        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        var coursesListForTeArci =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeArci).Result;

        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        var coursesListForTeDpm =
            GetExistingCoursesAsync(mergedDictionary: coursesForTeDpm).Result;

        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        var coursesListForTeTr = GetExistingCoursesAsync(mergedDictionary: coursesForTeTr).Result;

        // ------------------------------------------------------------------ //

        Console.WriteLine(value: "debug zone...");


        // ------------------------------------------------------------------ //
        //
        await SeedSchoolClassIfNotExists(code: "481228", acronym: "CET.GRSI.D.L.00",
            name: "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeGrsi, user: user);

        await SeedSchoolClassIfNotExists(code: "481228", acronym: "CET.SITE.DPO.92",
            name: "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeGrsi, user: user);

        await SeedSchoolClassIfNotExists(code: "481228", acronym: "CET.SITE.DPO.06",
            name: "Técnico/a Especialista em Gestão de Redes e Sistemas Informáticos (Pós-Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 19)),
            endHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 23)),
            courses: coursesListForTeGrsi, user: user);


        // 481227
        // Técnico/a Especialista em Aplicações Informáticas de Gestão
        await SeedSchoolClassIfNotExists(code: "481227", acronym: "CET.TEAIG.D.L.00",
            name: "Técnico/a Especialista em Aplicações Informáticas de Gestão",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeAig, user: user);


        // 481241
        // Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação
        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.TPSI.D.L.00",
            name: "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeTpsi, user: user);

        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.TPSI.N.L.00",
            name: "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-laboral)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 19)),
            endHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 23)),
            courses: coursesListForTeTpsi, user: user);

        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.SITE.DPO.91",
            name: "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeTpsi, user: user);

        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.SITE.DPO.04",
            name: "Técnico/a Especialista em Tecnologias e Programação de Sistemas de Informação (Pós-Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 19)),
            endHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 23)),
            courses: coursesListForTeTpsi, user: user);


        // 481344
        // Técnico/a Especialista em Cibersegurança
        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.SITE.DPO.99",
            name: "Técnico/a Especialista em Cibersegurança (Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeCs, user: user);
        await SeedSchoolClassIfNotExists(code: "481241", acronym: "CET.SITE.DPO.98",
            name: "Técnico/a Especialista em Cibersegurança (Pós-Laboral) (B-Learning)",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 19)),
            endHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 23)),
            courses: coursesListForTeCs, user: user);


        // 481390
        // Técnico/a Especialista em Gestão de Informação e Ciência dos Dados
        await SeedSchoolClassIfNotExists(code: "481390", acronym: "CET.SITE.99",
            name: "Técnico/a Especialista em Gestão de Informação e Ciência dos Dados",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeGicd, user: user);


        // 523229
        // Técnico/a Especialista em Automação, Robótica e Controlo Industrial
        await SeedSchoolClassIfNotExists(code: "523229", acronym: "CET.ARCI.N.L.00",
            name: "Técnico/a Especialista em Automação, Robótica e Controlo Industrial",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeArci, user: user);


        // 213240
        // Técnico/a Especialista em Desenvolvimento de Produtos Multimédia
        await SeedSchoolClassIfNotExists(code: "213240", acronym: "CET.ARCI.N.L.00",
            name: "Técnico/a Especialista em Desenvolvimento de Produtos Multimédia",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 19)),
            endHour: TimeSpan.Zero.Add(ts: TimeSpan.FromHours(value: 23)),
            courses: coursesListForTeDpm, user: user);


        // 523273
        // Técnico/a Especialista em Telecomunicações e Redes
        await SeedSchoolClassIfNotExists(code: "523273", acronym: "CET.TR.D.L.00",
            name: "Técnico/a Especialista em Telecomunicações e Redes",
            qnqLevel: 5, eqfLevel: 5,
            startDate: DateTime.Today.AddMonths(months: 1),
            endDate: DateTime.Today.AddMonths(months: 13),
            startHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 8).Add(ts: TimeSpan.FromMinutes(value: 30))),
            endHour: TimeSpan.Zero.Add(
                ts: TimeSpan.FromHours(value: 16).Add(ts: TimeSpan.FromMinutes(value: 30))),
            courses: coursesListForTeTr, user: user);


        // Add other school classes and courses here...

        // Add more courses here if needed

        Console.WriteLine(value: "Seeding completed.");
    }


    private static async Task<List<Course>> GetExistingCoursesAsync(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        // Filter out the courses that are already in the database and also present in the dictionary
        var existingCourses = _listOfCoursesFromDb
            .Where(predicate: course => mergedDictionary.ContainsKey(key: course.Code))
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
            predicate: s => s.Acronym == acronym && s.Code == code);


        // Calculate the total duration in hours from the sum of course hours
        var totalDurationInHours = courses.Sum(selector: c => c.Hours);


        // Calculate the total number of days needed for completion
        var totalDays =
            (int) Math.Ceiling(a: totalDurationInHours /
                                  ((endHour - startHour).TotalHours - 1));


        // Calculate the end date and time based on the total number of days and start date/hour
        endDate = startDate.AddDays(value: totalDays - 1).Date.Add(value: endHour);
        var durationPerDay = endHour - startHour;
        if (endDate.DayOfWeek == DayOfWeek.Saturday ||
            endDate.DayOfWeek == DayOfWeek.Sunday)
        {
            // Adjust the end date if it falls on a weekend
            var daysToAdd = endDate.DayOfWeek == DayOfWeek.Saturday ? 2 : 1;
            endDate =
                endDate.AddDays(value: daysToAdd).Date.Add(value: startHour + durationPerDay);
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

            await _dataContextInUse.SchoolClasses.AddAsync(entity: schoolClass);
            await _dataContextInUse.SaveChangesAsync();
        }
    }
}