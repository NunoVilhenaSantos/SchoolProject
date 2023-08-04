using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Data.Seeders.CoursesLists.CETs;
using SchoolProject.Web.Data.Seeders.CoursesLists.EFAs;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders.CoursesLists;

public class SeedDbCoursesList
{
    private static User _user;
    private static Random _random;

    private static IUserHelper _userHelper;
    private static ILogger<SeedDbCoursesList> _logger;


    private static DataContextMsSql _dataContextMsSql;


    private static Dictionary<string, (string, int, double)> _listOfCoursesToAdd = new();
    private static Dictionary<string, (string, int, double)> _listOfCoursesFromDb = new();



    // Disciplinas comuns da area de informática
    private static readonly Dictionary<string, (string, int, double)>
        FgcCommonDictionary = new()
        {
            {"5062", ("Língua portuguesa", 50, 5)},
            {"5063", ("Língua Inglesa", 50, 5)},
            {"5064", ("Matemática", 50, 5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeCsDictionary = new()
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {"5064", ("Matemática", 50, 5)},
            {"0683", ("Ética e deontologia profissionais", 25, 2.5)},
            {"3769", ("Probabilidades e estatística", 50, 5)},
            {"5065", ("Empresa - estrutura e funções", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeGicdDictionary = new()
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {"0683", ("Ética e deontologia profissionais", 25, 2.5)},
            {"5064", ("Matemática", 50, 5)},
            {"5143", ("Língua portuguesa - escrita de textos", 25, 2.5)},
            {"5144", ("Língua inglesa no quotidiano", 25, 2.5)},
            {"7825", ("Empresa – estrutura organizacional", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeArciDictionary = new()
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {
                "0349",
                ("Ambiente, Segurança, Higiene e Saúde no Trabalho - conceitos básicos",
                    25, 2.5)
            },
            {"5121", ("Gestão de projeto - eletrónica e automação", 25, 2.5)},
            {"5122", ("Organização e gestão da manutenção", 25, 2.5)},
            {"5123", ("Língua Inglesa no contexto profissional", 50, 5)},
            {"5124", ("Técnicas de expressão oral e escrita", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeDpmDictionary = new()
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {"5381", ("Comunicação e média", 25, 2.5)},
            {"5382", ("Publicidade e marketing", 25, 2.5)},
            {
                "5383",
                ("Inglês técnico aplicado à produção multimédia", 25, 2.5)
            },
            {"5384", ("Desenho e representações gráficas", 25, 2.5)},
            {"5385", ("Algoritmos e programação orientada a objetos", 25, 2.5)},
            {
                "5386",
                ("Direitos de autor, proteção de dados e propriedade industrial",
                    25, 2.5)
            }
        };

    private static readonly Dictionary<string, (string, int, double)>
        FgcTeTrDictionary = new()
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {"6402", ("Cálculo diferencial e integral", 50, 5)},
            {"6403", ("Comunicação oral e escrita", 25, 2.5)},
            {"6404", ("Inglês técnico aplicado às telecomunicações", 25, 2.5)},
            {"6405", ("Gestão", 25, 2.5)},
            {
                "6406",
                ("Técnicas aplicadas ao desenvolvimento humano nas organizações",
                    25, 2.5)
            }
        };


    public static void Initialize(DataContextMsSql dataContextMsSql)
    {
        //_user = user;
        _dataContextMsSql = dataContextMsSql;
    }


    internal static async Task AddingData(User user)
    {
        _user = user;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        TeTpsi();
        TeCs();
        TeAig();
        EfaTis();
        TeGrsi();
        TeGicd();
        TeArci();
        TeDpm();
        TeTr();

        // Adding the courses to the database
        await SaveMissingCourses();

        stopwatch.Stop();

        Console.WriteLine($"Tempo de execução: {stopwatch.Elapsed}");

        Console.WriteLine("debug zone...");
    }


    //internal static async void AddingData(SchoolProject.Web.Data.EntitiesMatrix.User user)
    //{
    //    _user = user;

    //    TeTpsi();
    //    TeCs();
    //    TeAig();
    //    EfaTis();
    //    TeGrsi();
    //    TeGicd();
    //    TeArci();
    //    TeDpm();
    //    TeTr();


    //    // Adding the courses to the database
    //    await SaveMissingCourses();


    //    Console.WriteLine("debug zone...");
    //}




    private static Dictionary<string, (string, int, double)>
        MergeDictionaries(
            Dictionary<string, (string, int, double)> commonCourses,
            Dictionary<string, (string, int, double)> specificCourses,
            Dictionary<string, (string, int, double)> workRelatedTraining
        )
    {
        var mergedDictionary =
            new Dictionary<string, (string, int, double)>(commonCourses);


        // Merge with the specificCourses dictionary
        foreach (var (key, value) in specificCourses)
            mergedDictionary
                .TryAdd(key, (value.Item1, value.Item2, value.Item3));


        // Merge with workRelatedTraining dictionary
        foreach (var (key, value) in workRelatedTraining)
            mergedDictionary
                .TryAdd(key, (value.Item1, value.Item2, value.Item3));


        // Accessing the values
        return mergedDictionary;
    }


    private static void PrintCourseInfo(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        foreach (var (courseNumber, courseInfo) in mergedDictionary)
        {
            var courseName = courseInfo.Item1;
            var courseHours = courseInfo.Item2;
            var creditPoints = courseInfo.Item3;

            Console.WriteLine(
                $"Course Number: {courseNumber}, " +
                $"Course Name: {courseName}, " +
                $"Hours: {courseHours}, " +
                $"Credit Points: {creditPoints}");
        }
    }


    private static void CalculateTotals(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        var totalHours = 0;
        double totalCreditPoints = 0;

        foreach (var (_, courseInfo) in mergedDictionary)
        {
            totalHours += courseInfo.Item2;
            totalCreditPoints += courseInfo.Item3;
        }

        Console.WriteLine(
            $"Total Hours: {totalHours}, " +
            $"Total Credit Points: {totalCreditPoints}");
    }


    private static Dictionary<string, (string, int, double)>
        MergeAndPrintCourseInfo(
            Dictionary<string, (string, int, double)> commonCourses,
            Dictionary<string, (string, int, double)> specificCourses,
            Dictionary<string, (string, int, double)> workRelatedTraining
        )
    {
        // Iniciar a medição do tempo
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var mergedDictionary = MergeDictionaries(
            commonCourses, specificCourses, workRelatedTraining);

        PrintCourseInfo(mergedDictionary);
        CalculateTotals(mergedDictionary);

        // Parar a medição do tempo
        stopwatch.Stop();

        // Tempo total decorrido (em milissegundos)
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Tempo de execução: {elapsedMilliseconds} ms");


        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeTpsi()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcCommonDictionary,
            ListCoursesTeTpsi.TeTpsiDictionary,
            ListsOfFct.Fct400);

        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeCs()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeCsDictionary,
            ListCoursesTeCs.TeCsDictionary,
            ListsOfFct.Fct560);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    // Técnico/a Especialista em Aplicações Informáticas de Gestão
    internal static Dictionary<string, (string, int, double)> TeAig()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcCommonDictionary,
            ListCoursesTeAig.TeAigDictionary,
            ListsOfFct.Fct400);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> EfaTis()
    {
        var mergedKcDictionary = MergeAndPrintCourseInfo(
            KeyCompetencies.FbCpCourses,
            KeyCompetencies.FbStcCourses,
            KeyCompetencies.FbClcCourses);

        var mergedDictionary = MergeAndPrintCourseInfo(
            mergedKcDictionary,
            ListCoursesTis.TisDictionary,
            ListsOfFct.Fct210);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeGrsi()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcCommonDictionary,
            ListCoursesTeGrsi.TeGrsiDictionary,
            ListsOfFct.Fct400);

        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeGicd()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeGicdDictionary,
            ListCoursesTeGicd.TeGicdDictionary,
            ListsOfFct.Fct400);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeArci()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeArciDictionary,
            ListCoursesTeArci.TeArciDictionary,
            ListsOfFct.Fct560);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeDpm()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeDpmDictionary,
            ListCoursesTeDpm.TeDpmDictionary,
            ListsOfFct.Fct500);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeTr()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeTrDictionary,
            ListCoursesTeTr.TeTrDictionary,
            ListsOfFct.Fct560);


        //SaveMissingCourses(mergedDictionary);
        AddMissingCourses(mergedDictionary);

        return mergedDictionary;
    }



    private static void AddMissingCourses(
            Dictionary<string, (string, int, double)> addCoursesToListOfCoursesToAdd
        )
    {
        //var mergedDictionary =
        //    new Dictionary<string, (string, int, double)>(commonCourses);


        // Merge with the _listOfCoursesToAdd dictionary
        foreach (var (key, value) in addCoursesToListOfCoursesToAdd)
            _listOfCoursesToAdd
                .TryAdd(key, (value.Item1, value.Item2, value.Item3));


        // Merge with workRelatedTraining dictionary
        //foreach (var (key, value) in workRelatedTraining)
        //    mergedDictionary
        //        .TryAdd(key, (value.Item1, value.Item2, value.Item3));
    }



    private static async 


    Task
SaveMissingCourses(
        //Dictionary<string, (string, int, double)> mergedDictionary
        )
    {
        // Get the list of existing course codes from the database
        var existingCourseCodes =
            await _dataContextMsSql.Courses.Select(c => c.Code).ToListAsync();


        // Filter out the courses that are already in the database
        var missingCourses =
            _listOfCoursesToAdd
                .Where(course =>
                    !existingCourseCodes.Contains(course.Key))
                .Select(course =>
                    new Course
                    {
                        Code = course.Key,
                        Name = course.Value.Item1,
                        Hours = course.Value.Item2,
                        CreditPoints = course.Value.Item3,
                        CreatedBy = _user,
                    })
                .ToList();


        Console.WriteLine($"Missing courses: {missingCourses.Count}");
        Console.WriteLine("Debug zone");


        // Save the missing courses to the database
        await _dataContextMsSql.Courses.AddRangeAsync(missingCourses);
        await _dataContextMsSql.SaveChangesAsync();
    }


    private static async Task<List<Course>> GetExistingCourses(
        Dictionary<string, (string, int, double)> mergedDictionary
        )
    {
        // Filter out the courses that are already in the database and also present in the dictionary
        var existingCourses =
            await _dataContextMsSql.Courses.AsAsyncEnumerable()
                .Where(course => mergedDictionary.ContainsKey(course.Code))
                .ToListAsync();

        var finalList = existingCourses.ToList();

        Console.WriteLine($"Existing courses: {existingCourses.Count}");
        Console.WriteLine("Debug zone");


        return finalList;
    }

}