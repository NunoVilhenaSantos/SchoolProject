using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Seeders.DisciplinesLists.CETs;
using SchoolProject.Web.Data.Seeders.DisciplinesLists.EFAs;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Seeders.DisciplinesLists;

public class SeedDBDisciplinesLists
{
    private static AppUser _appUser;
    // private static Random _random;

    // private static IUserHelper _userHelper;
    private static ILogger<SeedDBDisciplinesLists> _logger;

    // private static DataContextMsSql _dataContextMsSql;
    private static DataContextMySql _dataContextInUse;


    private static readonly Dictionary<string, (string, int, double)>
        _listOfDisciplinesToAdd = new();


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
            // Key: Discipline Code (string) -> Value: (UFCD, Horas) as a tuple
            {"5064", ("Matemática", 50, 5)},
            {"0683", ("Ética e deontologia profissionais", 25, 2.5)},
            {"3769", ("Probabilidades e estatística", 50, 5)},
            {"5065", ("Empresa - estrutura e funções", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeGicdDictionary = new()
        {
            // Key: Discipline Code (string) -> Value: (UFCD, Horas) as a tuple
            {"0683", ("Ética e deontologia profissionais", 25, 2.5)},
            {"5064", ("Matemática", 50, 5)},
            {"5143", ("Língua portuguesa - escrita de textos", 25, 2.5)},
            {"5144", ("Língua inglesa no quotidiano", 25, 2.5)},
            {"7825", ("Empresa – estrutura organizacional", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeArciDictionary = new()
        {
            // Key: Discipline Code (string) -> Value: (UFCD, Horas) as a tuple
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
            // Key: Discipline Code (string) -> Value: (UFCD, Horas) as a tuple
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
            // Key: Discipline Code (string) -> Value: (UFCD, Horas) as a tuple
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


    public static void Initialize(DataContextMySql dataContextInUse)
    {
        //_appUser = appUser;
        _dataContextInUse = dataContextInUse;
    }


    internal static async Task AddingData(AppUser appUser)
    {
        _appUser = appUser;

        // calcular o tempo decorrido para executar o método principal
        // Create a new timer with the name from variable "timerName" 
        TimeTracker.CreateTimer(TimeTracker.SeedDbDisciplinesName);

        // Start the timer "MyTimer"
        TimeTracker.StartTimer(TimeTracker.SeedDbDisciplinesName);

        EfaTis();
        TeCs();
        TeTr();
        TeDpm();
        TeAig();
        TeGrsi();
        TeGicd();
        TeArci();
        TeTpsi();

        // Adding the courses to the database
        await SaveMissingDisciplines();

        // Stop the timer "MyTimer"
        TimeTracker.StopTimer(TimeTracker.SeedDbDisciplinesName);

        // Get the elapsed time for the timer "MyTimer"
        var ts =
            TimeTracker.GetElapsedTime(TimeTracker.SeedDbDisciplinesName);

        TimeTracker.PrintTimerToConsole(TimeTracker.SeedDbDisciplinesName);

        Console.WriteLine("debug zone...");
    }


    private static Dictionary<string, (string, int, double)>
        MergeDictionaries(
            Dictionary<string, (string, int, double)> commonDisciplines,
            Dictionary<string, (string, int, double)> specificDisciplines,
            Dictionary<string, (string, int, double)> workRelatedTraining
        )
    {
        var mergedDictionary =
            new Dictionary<string, (string, int, double)>(commonDisciplines);


        // Merge with the specificDisciplines dictionary
        foreach (var (key, value) in specificDisciplines)
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
        foreach (var (disciplineNumber, disciplineInfo) in mergedDictionary)
        {
            var disciplineName = disciplineInfo.Item1;
            var disciplineHours = disciplineInfo.Item2;
            var creditPoints = disciplineInfo.Item3;

            Console.WriteLine(
                $"Discipline Number: {disciplineNumber}, " +
                $"Discipline Name: {disciplineName}, " +
                $"Hours: {disciplineHours}, " +
                $"Credit Points: {creditPoints}");
        }
    }


    private static void CalculateTotals(
        Dictionary<string, (string, int, double)> mergedDictionary)
    {
        var totalHours = 0;
        double totalCreditPoints = 0;

        foreach (var (_, disciplineInfo) in mergedDictionary)
        {
            totalHours += disciplineInfo.Item2;
            totalCreditPoints += disciplineInfo.Item3;
        }

        Console.WriteLine(
            $"Total Hours: {totalHours}, " +
            $"Total Credit Points: {totalCreditPoints}");
    }


    private static Dictionary<string, (string, int, double)>
        MergeAndPrintDisciplinesInfo(
            Dictionary<string, (string, int, double)> commonCourses,
            Dictionary<string, (string, int, double)> specificCourses,
            Dictionary<string, (string, int, double)> workRelatedTraining
        )
    {
        // calcular o tempo decorrido para executar o método principal
        // Create a new timer with the name from variable "timerName" 
        TimeTracker.CreateTimer(TimeTracker.SeedDbDisciplinesMPInfoName);

        // Start the timer "MyTimer"
        TimeTracker.StartTimer(TimeTracker.SeedDbDisciplinesMPInfoName);


        var mergedDictionary = MergeDictionaries(
            commonCourses, specificCourses, workRelatedTraining);

        PrintCourseInfo(mergedDictionary);
        CalculateTotals(mergedDictionary);


        // Parar a medição do tempo
        TimeTracker.StopTimer(TimeTracker.SeedDbDisciplinesMPInfoName);

        // Get the elapsed time for the timer "MyTimer"
        var ts =
            TimeTracker.GetElapsedTime(TimeTracker.SeedDbDisciplinesMPInfoName);

        TimeTracker.PrintTimerToConsole(TimeTracker
            .SeedDbDisciplinesMPInfoName);

        Console.WriteLine("debug zone...");


        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeTpsi()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcCommonDictionary,
            ListCetTeTpsi.TeTpsiDictionary,
            ListsOfFct.Fct400);

        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeCs()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcTeCsDictionary,
            ListCetTeCs.TeCsDictionary,
            ListsOfFct.Fct560);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    // Técnico/a Especialista em Aplicações Informáticas de Gestão
    internal static Dictionary<string, (string, int, double)> TeAig()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcCommonDictionary,
            ListCetTeAig.TeAigDictionary,
            ListsOfFct.Fct400);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    internal static Dictionary<string, (string, int, double)> EfaTis()
    {
        var mergedKcDictionary = MergeAndPrintDisciplinesInfo(
            KeyCompetencies.FbCpCourses,
            KeyCompetencies.FbStcCourses,
            KeyCompetencies.FbClcCourses);

        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            mergedKcDictionary,
            ListEfaTis.TisDictionary,
            ListsOfFct.Fct210);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeGrsi()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcCommonDictionary,
            ListCetTeGrsi.TeGrsiDictionary,
            ListsOfFct.Fct400);

        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeGicd()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcTeGicdDictionary,
            ListCetTeGicd.TeGicdDictionary,
            ListsOfFct.Fct400);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeArci()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcTeArciDictionary,
            ListCetTeArci.TeArciDictionary,
            ListsOfFct.Fct560);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeDpm()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcTeDpmDictionary,
            ListCetTeDpm.TeDpmDictionary,
            ListsOfFct.Fct500);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeTr()
    {
        var mergedDictionary = MergeAndPrintDisciplinesInfo(
            FgcTeTrDictionary,
            ListCetTeTr.TeTrDictionary,
            ListsOfFct.Fct560);


        //SaveMissingDisciplines(mergedDictionary);
        AddMissingDisciplines(mergedDictionary);

        return mergedDictionary;
    }


    private static void AddMissingDisciplines(
        Dictionary<string, (string, int, double)> addMissingDisciplines
    )
    {
        //var mergedDictionary =
        //    new Dictionary<string, (string, int, double)>(commonDisciplines);


        // Merge with the _listOfDisciplinesToAdd dictionary
        foreach (var (key, value) in addMissingDisciplines)
            _listOfDisciplinesToAdd
                .TryAdd(key, (value.Item1, value.Item2, value.Item3));


        // Merge with workRelatedTraining dictionary
        //foreach (var (key, value) in workRelatedTraining)
        //    mergedDictionary
        //        .TryAdd(key, (value.Item1, value.Item2, value.Item3));
    }


    private static async Task SaveMissingDisciplines(
        //Dictionary<string, (string, int, double)> mergedDictionary
    )
    {
        // Get the list of existing discipline codes from the database
        var existingDisciplinesCodes =
            await _dataContextInUse.Disciplines.Select(c => c.Code)
                .ToListAsync();


        // Filter out the courses that are already in the database
        var missingDisciplines =
            _listOfDisciplinesToAdd
                .Where(discipline =>
                    !existingDisciplinesCodes.Contains(discipline.Key))
                .Select(discipline =>
                    new Discipline
                    {
                        Code = discipline.Key,
                        Name = discipline.Value.Item1,
                        Hours = discipline.Value.Item2,
                        CreditPoints = discipline.Value.Item3,
                        CreatedBy = _appUser,
                        ProfilePhotoId = default,
                        Description = string.Empty,
                    })
                .ToList();


        Console.WriteLine($"Missing Disciplines: {missingDisciplines.Count}");
        Console.WriteLine("Debug zone");


        // Save the missing courses to the database
        await _dataContextInUse.Disciplines.AddRangeAsync(missingDisciplines);
        await _dataContextInUse.SaveChangesAsync();
    }


    private static async Task<List<Discipline>> GetExistingDisciplines(
        Dictionary<string, (string, int, double)> mergedDictionary
    )
    {
        // Filter out the courses that are already in the database and also present in the dictionary
        var existingDisciplines =
            await _dataContextInUse.Disciplines.AsAsyncEnumerable()
                .Where(discipline =>
                    mergedDictionary.ContainsKey(discipline.Code))
                .ToListAsync();

        var finalList = existingDisciplines.ToList();

        Console.WriteLine($"Existing Disciplines: {existingDisciplines.Count}");
        Console.WriteLine("Debug zone");


        return finalList;
    }
}