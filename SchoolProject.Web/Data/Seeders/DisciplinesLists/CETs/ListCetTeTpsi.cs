namespace SchoolProject.Web.Data.Seeders.DisciplinesLists.CETs;

public record ListCetTeTpsi
{
    internal static readonly Dictionary<string, (string, int, double)>
        TeTpsiDictionary = new()
        {
            // Key: Discipline Code (string) -> Value: (Nº, UFCD obrigatórias, Horas, Pontos de crédito) as a tuple
            {"5065", ("Empresa - estrutura e funções", 25, 2.25)},
            {"5407", ("Sistemas de informação - fundamentos", 25, 2.25)},
            {"5408", ("Sistemas de informação - conceção", 25, 2.25)},
            {"5409", ("Engenharia de software", 25, 2.25)},
            {"5410", ("Bases de dados - conceitos", 25, 2.25)},
            {"5411", ("Bases de dados - sistemas de gestão", 25, 2.25)},
            {
                "5085",
                ("Criação de estrutura de base de dados em SQL", 25, 2.25)
            },
            {"5086", ("Programação em SQL", 25, 2.25)},
            {"5089", ("Programação - Algoritmos", 25, 2.25)},
            {
                "5412",
                ("Programação de computadores - estruturada", 50, 4.50)
            },
            {
                "5413",
                ("Programação de computadores - orientada a objetos", 50,
                    4.50)
            },
            {
                "5414",
                ("Programação para a WEB - cliente (client-side)", 50, 4.50)
            },
            {"5415", ("WEB - hipermédia e acessibilidades", 25, 2.25)},
            {"5416", ("WEB - ferramentas multimédia", 25, 2.25)},
            {
                "5417",
                ("Programação para a WEB - servidor (server-side)", 50,
                    4.50)
            },
            {"5418", ("Redes de comunicação de dados", 25, 2.25)},
            {"5419", ("Segurança em sistemas informáticos", 25, 2.25)},
            {"5116", ("Sistemas operativos open source", 25, 2.25)},
            {
                "5114",
                ("Sistema operativo servidor (plataforma proprietária)", 25,
                    2.25)
            },
            {
                "5420",
                ("Integração de sistemas de informação - conceitos", 25,
                    2.25)
            },
            {
                "5421",
                ("Integração de sistemas de informação - tecnologias e níveis de Integração",
                    50, 4.50)
            },
            {
                "5422",
                ("Integração de sistemas de informação - ferramentas", 25,
                    2.25)
            },
            {"5423", ("Acesso móvel a sistemas de informação", 50, 4.50)},
            {
                "5424",
                ("Planeamento e gestão de projetos de sistemas de informação",
                    25, 2.25)
            },
            {
                "5425",
                ("Projeto de tecnologias e programação de sistemas de informação",
                    50, 4.50)
            },
            {
                "11027",
                ("Desenvolvimento de aplicações móveis (plataforma iOS)",
                    50,
                    4.50)
            }
        };
}