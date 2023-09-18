namespace SchoolProject.Web.Data.Seeders.DisciplinesLists;

public record ListsOfFct
{
    // Formação em Contexto de Trabalho
    internal static readonly Dictionary<string, (string, int, double)> Fct210 =
        new() {{"FCT210", ("Formação em Contexto de Trabalho", 210, 20)}};


    internal static readonly Dictionary<string, (string, int, double)> Fct350 =
        new() {{"FCT350", ("Formação em Contexto de Trabalho", 350, 15)}};


    internal static readonly Dictionary<string, (string, int, double)> Fct400 =
        new() {{"FCT400", ("Formação em Contexto de Trabalho", 400, 15)}};


    internal static readonly Dictionary<string, (string, int, double)> Fct500 =
        new() {{"FCT500", ("Formação em Contexto de Trabalho", 500, 15)}};


    internal static readonly Dictionary<string, (string, int, double)> Fct560 =
        new() {{"FCT560", ("Formação em Contexto de Trabalho", 560, 15)}};
}