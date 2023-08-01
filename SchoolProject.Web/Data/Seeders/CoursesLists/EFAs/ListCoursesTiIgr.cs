namespace SchoolProject.Web.Data.Seeders.CoursesLists.EFAs;

public record ListCoursesTiIgr
{
    private static Dictionary<string, (string, int, double)> TiIgrDictionary =
        new Dictionary<string, (string, int, double)>
        {
            {"0769", ("Arquitetura interna do computador", 25, 2.25)},
            {"0770", ("Dispositivos e periféricos", 25, 2.25)},
            {"0771", ("Conexões de rede", 25, 2.25)},
            {
                "0772",
                ("Sistemas operativos - instalação e configuração", 25, 2.25)
            },
            {"0773", ("Rede local - instalação", 25, 2.25)},
            {"0774", ("Rede local - instalação de software base", 50, 4.50)},
            {"0775", ("Rede local - administração", 50, 4.50)},
            {"0776", ("Sistema de informação da empresa", 25, 2.25)},
            {"0778", ("Folha de cálculo", 50, 4.50)},
            {"0754", ("Processador de texto", 50, 4.50)},
            {"0779", ("Utilitário de apresentação gráfica", 25, 2.25)},
            {"0780", ("Aplicações de gestão administrativa", 50, 4.50)},
            {"0781", ("Análise de sistemas de informação", 50, 4.50)},
            {
                "0782",
                ("Programação em C/C++ - estrutura básica e conceitos fundamentais",
                    50, 4.50)
            },
            {"0783", ("Programação em C/C++ - ciclos e decisões", 50, 4.50)},
            {"0784", ("Programação em C/C++ - funções e estruturas", 50, 4.50)},
            {"0785", ("Programação em C/C++ - formas complexas", 50, 4.50)},
            {
                "0786",
                ("Instalação e configuração de sistemas de gestão de bases de dados",
                    50, 4.50)
            },
            {"0787", ("Administração de bases de dados", 50, 4.50)},
            {
                "0788",
                ("Instalação e administração de servidores WEB", 50, 4.50)
            },
            {"0789", ("Fundamentos de linguagem JAVA", 50, 4.50)},
            {"10791", ("Desenvolvimento de aplicações web em JAVA", 50, 4.50)},
            {"0791", ("Programação em JAVA - avançada", 50, 4.50)},
            {"0792", ("Criação de páginas para a web em hipertexto", 25, 2.25)},
            {"0793", ("Scripts CGI e folhas de estilo", 25, 2.25)}
        };
}