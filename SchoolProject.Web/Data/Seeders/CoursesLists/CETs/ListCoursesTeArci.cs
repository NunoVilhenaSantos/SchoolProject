namespace SchoolProject.Web.Data.Seeders.CoursesLists.CETs;

public record ListCoursesTeArci()
{
    internal static Dictionary<string, (string, int, double)> TeArciDictionary =
        new Dictionary<string, (string, int, double)>
        {
            // Key: Course Code (string) -> Value: (Nº, UFCD obrigatórias, Horas, Pontos de crédito) as a tuple
            {"5125", ("Técnicas de programação", 50, 4.50)},
            {"5126", ("Eletrónica industrial", 50, 4.50)},
            {
                "5127",
                ("Máquinas elétricas - motores e controladores de velocidade",
                    50, 4.50)
            },
            {"5128", ("Pneutrónica", 50, 4.50)},
            {"5129", ("Automação", 50, 4.50)},
            {
                "5130",
                ("Automação industrial - automatos programáveis", 50, 4.50)
            },
            {"5131", ("Controlo industrial - fundamentos", 50, 4.50)},
            {"5132", ("Controlo industrial - avançado", 50, 4.50)},
            {"5133", ("Introdução ao CIM", 50, 4.50)},
            {"5134", ("Robótica - fundamentos", 50, 4.50)},
            {"5135", ("Robótica - avançado", 25, 2.25)},
            {"5136", ("Sistemas de micro controladores", 50, 4.50)},
            {
                "5137",
                ("Instrumentação industrial - conceitos básicos", 50, 4.50)
            },
            {"5138", ("Instrumentação industrial - avançado", 25, 2.25)},
            {"5139", ("Domótica - projeto", 50, 4.50)},
            {"5140", ("Projeto - bases", 50, 4.50)},
            {
                "5141",
                ("Projeto integrado de automação e controlo - implementação",
                    50, 4.50)
            },
            {
                "5142",
                ("Projeto integrado de automação e controlo - otimização", 50,
                    4.50)
            }
        };
}