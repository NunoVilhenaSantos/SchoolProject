using SchoolProject.Web.Data.Seeders.CoursesLists.CETs;

namespace SchoolProject.Web.Data.Seeders.CoursesLists;

public static class SeedDbCoursesList
{
    // Disciplinas comuns da area de informática
    internal static Dictionary<string, (string, int, double)>
        DicitionaryCommonCourses = new()
        {
            {"5062", ("Língua portuguesa", 50, 4.5)},
            {"5063", ("Língua Inglesa", 50, 4.5)},
            {"5064", ("Matemática", 50, 4.5)}
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


    // Técnico/a Especialista em Aplicações Informáticas de Gestão
    internal static Dictionary<string, (string, int, double)> TeAig()
    {
        var mergedDictionary = MergeDictionaries(
            DicitionaryCommonCourses,
            TeaigListOfCourses.TeAigDictionary,
            ListsOfFct.Fct400);


        // Accessing the values
        foreach (
            var (courseNumber, courseInfo) in mergedDictionary)
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


        // Calculate the total hours and credit points
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


        return mergedDictionary;
    }
}