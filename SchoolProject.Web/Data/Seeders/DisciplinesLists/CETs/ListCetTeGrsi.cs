namespace SchoolProject.Web.Data.Seeders.DisciplinesLists.CETs;

public abstract record ListCetTeGrsi
{
    internal static readonly Dictionary<string, (string, int, double)>
        TeGrsiDictionary = new()
        {
            // Key: Discipline Code (string) -> Value: (UFCD obrigatórias, Horas, Pontos de crédito) as a tuple
            {"5097", ("História da informática", 25, 2.25)},
            {"5098", ("Arquitetura de hardware", 25, 2.25)},
            {"5099", ("Montagem de hardware", 25, 2.25)},
            {"5100", ("Deteção de avarias", 25, 2.25)},
            {"5101", ("Hardware e redes de computadores", 25, 2.25)},
            {"5102", ("Redes de computadores (avançado)", 25, 2.25)},
            {
                "5103",
                ("Avaliação das necessidades de rede numa organização", 25,
                    2.25)
            },
            {"5104", ("Instalação de redes locais", 50, 4.50)},
            {"5105", ("Arquitetura cliente - servidor", 25, 2.25)},
            {"5106", ("Serviços de rede", 25, 2.25)},
            {"5107", ("Servidor de dados", 25, 2.25)},
            {
                "5108",
                ("Configuração avançada de sistemas operativos servidores", 25,
                    2.25)
            },
            {"5109", ("Políticas de segurança", 50, 4.50)},
            {"5110", ("Servidor de correio eletrónico", 25, 2.25)},
            {
                "5111",
                ("Configuração de serviços num servidor linux", 50, 4.50)
            },
            {"5112", ("Introdução aos sistemas operativos", 25, 2.25)},
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
            {"5115", ("Sistema operativo servidor open source", 25, 2.25)},
            {"5116", ("Sistemas operativos open source", 25, 2.25)},
            {
                "5080",
                ("Gestão e manipulação avançada de aplicações informáticas de processamento de texto",
                    25, 2.25)
            },
            {
                "5081",
                ("Gestão e manipulação avançada de aplicações informáticas de folha de cálculo",
                    25, 2.25)
            },
            {
                "5117",
                ("Primeiros conceitos de programação e algoritmia e estruturas de controlo num programa informático",
                    25, 2.25)
            },
            {
                "5091",
                ("Programação estruturada e tipos de dados", 25, 2.25)
            },
            {
                "5118",
                ("Programação orientada a objetos - introdução", 25, 2.25)
            },
            {
                "5119",
                ("Estrutura de dados estática, composta e dinâmica", 50,
                    4.50)
            },
            {
                "5083",
                ("Análise de sistemas e estruturação de bases de dados", 25,
                    2.25)
            },
            {
                "5085",
                ("Criação de estrutura de base de dados em SQL", 25, 2.25)
            },
            {"5086", ("Programação em SQL", 25, 2.25)}
        };
}