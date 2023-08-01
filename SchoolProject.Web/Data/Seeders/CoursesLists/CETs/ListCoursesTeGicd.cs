namespace SchoolProject.Web.Data.Seeders.CoursesLists.CETs;

public record ListCoursesTeGicd()
{
    internal static Dictionary<string, (string, int, double)> TeGicdDictionary =
        new Dictionary<string, (string, int, double)>
        {
            // Key: Course Code (string) -> Value: (Nº, UFCD obrigatórias, Horas, Pontos de crédito) as a tuple
            {"5745", ("Inglês técnico", 50, 4.50)},
            {"10788", ("Fundamentos da linguagem SQL", 25, 2.25)},
            {"9950", ("Conceitos fundamentais de programação", 50, 4.50)},
            {"2166", ("Estatística aplicada à gestão", 50, 4.50)},
            {"10796", ("Gestão de informação", 25, 2.25)},
            {"10797", ("Gestão e armazenamento de dados", 50, 4.50)},
            {
                "10672",
                ("Introdução à utilização e proteção dos dados pessoais",
                    25, 2.25)
            },
            {"10805", ("Programação em Python", 50, 4.50)},
            {
                "10806",
                ("Linguagens de scripting e linha de comandos", 25, 2.25)
            },
            {
                "10807",
                ("Princípios básicos da análise exploratória de dados", 25,
                    2.25)
            },
            {
                "10808",
                ("Limpeza e transformação de dados em Python", 25, 2.25)
            },
            {"10809", ("Visualização de dados em Python", 25, 2.25)},
            {
                "10810",
                ("Fundamentos do desenvolvimento de modelos analíticos em Python",
                    25, 2.25)
            },
            {"10811", ("Projeto de análise de dados", 50, 4.50)},
            {"10798", ("Ingestão de dados", 25, 2.25)},
            {"10799", ("Modelação de dados", 25, 2.25)},
            {"10800", ("Transformação de dados", 25, 2.25)},
            {"10801", ("Visualização de dados", 25, 2.25)},
            {"10802", ("Storytelling com dados", 50, 4.50)},
            {"10803", ("Análise avançada de dados", 25, 2.25)},
            {"10804", ("Projeto de business intelligence", 50, 4.50)}
        };
}