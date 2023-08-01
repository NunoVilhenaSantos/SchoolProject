namespace SchoolProject.Web.Data.Seeders.CoursesLists.CETs;

public record ListCoursesTeDpm()
{
    internal static Dictionary<string, (string, int, double)> TeDpmDictionary =
        new Dictionary<string, (string, int, double)>
        {
            // Key: Course Code (string) -> Value: (UFCD, Horas, Pontos de crédito) as a tuple
            {"5387", ("Técnicas de design", 50, 4.50)},
            {"5388", ("Design multimédia", 50, 4.50)},
            {"5389", ("Imagem digital", 25, 2.25)},
            {"5390", ("Ilustração digital", 25, 2.25)},
            {"5391", ("Desenho bitmap", 25, 2.25)},
            {"5392", ("Imagem vetorial", 50, 4.50)},
            {"5393", ("Desenho de sítios Web", 25, 2.25)},
            {"5394", ("Técnicas avançadas de programação Web", 50, 4.50)},
            {"5395", ("Tecnologias multimédia na internet", 50, 4.50)},
            {"5396", ("Desenho e administração de bases de dados", 50, 4.50)},
            {"5397", ("Sistemas de gestão de conteúdos", 25, 2.25)},
            {"5398", ("Aplicações em tecnologia Web 2.0", 25, 2.25)},
            {"5399", ("Animação multimédia", 50, 4.50)},
            {"5400", ("Animação 3D", 25, 2.25)},
            {"5401", ("Modelação 3D", 50, 4.50)},
            {"5402", ("Iluminação e \"renderização\" 3D", 25, 2.25)},
            {"5403", ("Composição e efeitos audiovisuais", 25, 2.25)},
            {"0145", ("Som/áudio - captação, registo e edição", 50, 4.50)},
            {"0146", ("Imagem/vídeo - captação, registo e edição", 50, 4.50)},
            {"5404", ("Pós-produção de vídeo", 50, 4.50)},
            {"5405", ("Metodologia e gestão de projetos multimédia", 50, 4.50)},
            {"5406", ("Projeto integrado de multimédia", 25, 2.25)}
        };
}