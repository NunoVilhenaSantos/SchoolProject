namespace SchoolProject.Web.Data.Seeders.CoursesLists;

public static class ListsOfFct
{
    // Formação em Contexto de Trabalho
    internal static Dictionary<string, (string, int, double)> Fct300 =
        new() {{"FCT400", ("Formação em Contexto de Trabalho", 300, 10)}};

    internal static Dictionary<string, (string, int, double)> Fct350 =
        new() {{"FCT500", ("Formação em Contexto de Trabalho", 350, 12)}};

    internal static Dictionary<string, (string, int, double)> Fct400 =
        new() {{"FCT400", ("Formação em Contexto de Trabalho", 400, 15)}};

    internal static Dictionary<string, (string, int, double)> Fct500 =
        new() {{"FCT500", ("Formação em Contexto de Trabalho", 500, 18)}};
}