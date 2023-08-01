namespace SchoolProject.Web.Data.Seeders.CoursesLists.CETs;

public abstract record ListCoursesTeCs
{
    internal static readonly Dictionary<string, (string, int, double)>
        TeCsDictionary = new()
        {
            // Key: Course Code (string) -> Value: (Nº, UFCD obrigatórias, Horas, Pontos de crédito) as a tuple
            {
                "5117",
                ("Primeiros conceitos de programação e algoritmia e estruturas de controlo num programa informático",
                    25, 2.25)
            },
            {"9187", ("Legislação, segurança e privacidade", 25, 2.25)},
            {"5745", ("Inglês técnico", 50, 4.50)},
            {"5089", ("Programação - Algoritmos", 25, 2.25)},
            {"5410", ("Bases de dados - conceitos", 25, 2.25)},
            {
                "5113",
                ("Sistema operativo cliente (plataforma proprietária)", 25,
                    2.25)
            },
            {
                "5114",
                ("Sistema operativo servidor (plataforma proprietária)", 25,
                    2.25)
            },
            {"5101", ("Hardware e redes de computadores", 25, 2.25)},
            {"5102", ("Redes de computadores (avançado)", 25, 2.25)},
            {"5104", ("Instalação de redes locais", 50, 4.50)},
            {"5106", ("Serviços de rede", 25, 2.25)},
            {
                "5892",
                ("Modelos de gestão de redes e de suporte a clientes", 25,
                    2.25)
            },
            {"9188", ("Fundamentos de cibersegurança", 25, 2.25)},
            {"9189", ("Tecnologias de análise de evidências", 50, 4.50)},
            {
                "9190",
                ("Introdução à programação aplicada à cibersegurança", 25,
                    2.25)
            },
            {
                "9191",
                ("Introdução às técnicas de análise de evidências", 50,
                    4.50)
            },
            {"9192", ("Análise de vulnerabilidades – iniciação", 50, 4.50)},
            {
                "9193",
                ("Análise de vulnerabilidades - desenvolvimento", 50, 4.50)
            },
            {
                "9194",
                ("Introdução à cibersegurança e à ciberdefesa", 50, 4.50)
            },
            {
                "9195",
                ("Enquadramento operacional da cibersegurança", 50, 4.50)
            },
            {"9196", ("Cibersegurança ativa", 50, 4.50)},
            {"9197", ("Wargamming", 50, 4.50)}
        };
}