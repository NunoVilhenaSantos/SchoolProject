using System.Diagnostics;
using SchoolProject.Web.Data.Seeders.CoursesLists.CETs;
using SchoolProject.Web.Data.Seeders.CoursesLists.EFAs;

namespace SchoolProject.Web.Data.Seeders.CoursesLists;

public static class SeedDbCoursesList
{
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
        FgcTeGicdDictionary = new Dictionary<string, (string, int, double)>
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas) as a tuple
            {"0683", ("Ética e deontologia profissionais", 25, 2.5)},
            {"5064", ("Matemática", 50, 5)},
            {"5143", ("Língua portuguesa - escrita de textos", 25, 2.5)},
            {"5144", ("Língua inglesa no quotidiano", 25, 2.5)},
            {"7825", ("Empresa – estrutura organizacional", 25, 2.5)}
        };


    private static readonly Dictionary<string, (string, int, double)>
        FgcTeArciDictionary = new Dictionary<string, (string, int, double)>
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
        FgcTeDpmDictionary = new Dictionary<string, (string, int, double)>
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

    public static readonly Dictionary<string, (string, int, double)>
        FgcTeTrDictionary = new Dictionary<string, (string, int, double)>
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
            },
        };


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

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeCs()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeCsDictionary,
            ListCoursesTeCs.TeCsDictionary,
            ListsOfFct.Fct560);

        return mergedDictionary;
    }


    // Técnico/a Especialista em Aplicações Informáticas de Gestão
    internal static Dictionary<string, (string, int, double)> TeAig()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcCommonDictionary,
            ListCoursesTeAig.TeAigDictionary,
            ListsOfFct.Fct400);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> EfaTis()
    {
        var mergedKcDictionary = MergeAndPrintCourseInfo(
            KeyCompetencies.FbCPCourses,
            KeyCompetencies.FbSTCCourses,
            KeyCompetencies.FbCLCCourses);

        var mergedDictionary = MergeAndPrintCourseInfo(
            mergedKcDictionary,
            ListCoursesTis.TisDictionary,
            ListsOfFct.Fct210);

        return mergedDictionary;
    }


    public static Dictionary<string, (string, int, double)> TeGrsi()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcCommonDictionary,
            ListCoursesTeGrsi.TeGrsiDictionary,
            ListsOfFct.Fct400);

        return mergedDictionary;
    }

    public static Dictionary<string, (string, int, double)> TeGicd()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeGicdDictionary,
            ListCoursesTeGicd.TeGicdDictionary,
            ListsOfFct.Fct400);

        return mergedDictionary;
    }

    public static Dictionary<string, (string, int, double)> TeArci()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeArciDictionary,
            ListCoursesTeArci.TeArciDictionary,
            ListsOfFct.Fct560);

        return mergedDictionary;
    }

    public static Dictionary<string, (string, int, double)> TeDpm()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeDpmDictionary,
            ListCoursesTeDpm.TeDpmDictionary,
            ListsOfFct.Fct500);

        return mergedDictionary;
    }

    public static Dictionary<string, (string, int, double)> TeTr()
    {
        var mergedDictionary = MergeAndPrintCourseInfo(
            FgcTeTrDictionary,
            ListCoursesTeTr.TeTrDictionary,
            ListsOfFct.Fct560);

        return mergedDictionary;
    }
}