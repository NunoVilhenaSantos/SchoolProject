namespace SchoolProject.Web.Data.Seeders.CoursesLists.EFAs;

public record KeyCompetencies
{
    // Cidadania e Profissionalidade
    public static readonly Dictionary<string, (string, int, double)>
        FbCpCourses = new()
        {
            {"CP_1", ("Liberdade e responsabilidade democráticas", 50, 5)},
            {"CP_4", ("Processos identitários", 50, 5)},
            {"CP_5", ("Deontologia e princípios éticos", 50, 5)}
        };

    // Sociedade, Tecnologia e Ciência
    public static readonly Dictionary<string, (string, int, double)>
        FbStcCourses = new()
        {
            {"STC_5", ("Redes de informação e comunicação", 50, 5)},
            {"STC_6", ("Modelos de urbanismo e mobilidade", 50, 5)},
            {
                "STC_7",
                ("Sociedade, tecnologia e ciência - fundamentos", 50, 5)
            }
        };

    // Cultura, Língua e Comunicação
    public static readonly Dictionary<string, (string, int, double)>
        FbClcCourses = new()
        {
            {"CLC_5", ("Cultura, comunicação e média", 50, 5)},
            {"CLC_6", ("Culturas de urbanismo e mobilidade", 50, 5)},
            {
                "CLC_7",
                ("Fundamentos de cultura, língua e comunicação", 50, 5)
            },
            {"CPPRA", ("PORTEFÓLIO REFLEXIVO DE APRENDIZAGENS", 85, 25)}
        };

    // Cidadania e Profissionalidade
    public static readonly Dictionary<string, (string, int, double)>
        CidadaniaProfissionalidadeCourses = new()
        {
            {"CP_1", ("Liberdade e responsabilidade democráticas", 50, 5)},
            {"CP_2", ("Processos sociais de mudança", 50, 5)},
            {"CP_3", ("Reflexão e crítica", 50, 5)},
            {"CP_4", ("Processos identitários", 50, 5)},
            {"CP_5", ("Deontologia e princípios éticos", 50, 5)},
            {"CP_6", ("Tolerância e mediação", 50, 5)},
            {"CP_7", ("Processos e técnicas de negociação", 50, 5)},
            {"CP_8", ("Construção de projetos pessoais e sociais", 50, 5)}
        };

    // Sociedade, Tecnologia e Ciência
    public static readonly Dictionary<string, (string, int, double)>
        SociedadeTecnologiaCienciaCourses = new()
        {
            {
                "STC_1",
                ("Equipamentos - princípios de funcionamento", 50, 5)
            },
            {"STC_2", ("Sistemas ambientais", 50, 5)},
            {"STC_3", ("Saúde - comportamentos e instituições", 50, 5)},
            {"STC_4", ("Relações económicas", 50, 5)},
            {"STC_5", ("Redes de informação e comunicação", 50, 5)},
            {"STC_6", ("Modelos de urbanismo e mobilidade", 50, 5)},
            {
                "STC_7",
                ("Sociedade, tecnologia e ciência - fundamentos", 50, 5)
            }
        };

    // Cultura, Língua e Comunicação
    public static readonly Dictionary<string, (string, int, double)>
        CulturaLinguaComunicacaoCourses = new()
        {
            {
                "CLC_1",
                ("Equipamentos - impactos culturais e comunicacionais", 50, 5)
            },
            {"CLC_2", ("Culturas ambientais", 50, 5)},
            {"CLC_3", ("Saúde - língua e comunicação", 50, 5)},
            {"CLC_4", ("Comunicação nas organizações", 50, 5)},
            {"CLC_5", ("Cultura, comunicação e média", 50, 5)},
            {"CLC_6", ("Culturas de urbanismo e mobilidade", 50, 5)},
            {
                "CLC_7",
                ("Fundamentos de cultura, língua e comunicação", 50, 5)
            },
            {
                "CLC_LEI_1",
                ("Língua estrangeira - iniciação - inglês", 50, 5)
            },
            {
                "CLC_LEI_2",
                ("Língua estrangeira - iniciação - francês", 50, 5)
            },
            {
                "CLC_LEI_3",
                ("Língua estrangeira - iniciação - alemão", 50, 5)
            },
            {
                "CLC_LEI_4",
                ("Língua estrangeira - iniciação - espanhol", 50, 5)
            },
            {
                "CLC_LEI_5",
                ("Língua estrangeira - iniciação - italiano", 50, 5)
            },
            {
                "CLC_LEC_1",
                ("Língua estrangeira - continuação - inglês", 50, 5)
            },
            {
                "CLC_LEC_2",
                ("Língua estrangeira - continuação - francês", 50, 5)
            },
            {
                "CLC_LEC_3",
                ("Língua estrangeira - continuação - alemão", 50, 5)
            },
            {
                "CLC_LEC_4",
                ("Língua estrangeira - continuação - espanhol", 50, 5)
            },
            {
                "CLC_LEC_5",
                ("Língua estrangeira - continuação - italiano", 50, 5)
            }
        };
}