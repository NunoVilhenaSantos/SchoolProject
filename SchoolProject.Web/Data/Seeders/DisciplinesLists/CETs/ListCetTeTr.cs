namespace SchoolProject.Web.Data.Seeders.DisciplinesLists.CETs;

public record ListCetTeTr
{
    internal static readonly Dictionary<string, (string, int, double)>
        TeTrDictionary = new()
        {
            // Key: Discipline Code (string) -> Value: (UFCD, Horas, Pontos de crédito) as a tuple
            {"6027", ("Circuitos sequenciais síncronos", 50, 4.50)},
            {"6188", ("Transmissão de sinais", 50, 4.50)},
            {"6189", ("Interfaces e suportes de transmissão", 50, 4.50)},
            {
                "6090",
                ("Instalações ITED - elaboração de projeto", 50, 4.50)
            },
            {
                "6133",
                ("Redes de comunicações - dimensionamento de redes IP", 25,
                    2.25)
            },
            {"6134", ("Redes locais", 50, 4.50)},
            {"6190", ("Sistema operativo Windows", 50, 4.50)},
            {"6191", ("Serviços de rede Windows", 25, 2.25)},
            {"6135", ("Redes de operador", 25, 2.25)},
            {"6136", ("Redes wireless", 25, 2.25)},
            {"6137", ("Redes de nova geração", 50, 4.50)},
            {
                "6138",
                ("Redes de comunicações - configuração de routers", 50,
                    4.50)
            },
            {"6194", ("Televisão digital", 25, 2.25)},
            {"6195", ("Projeto de comunicações e redes", 50, 4.50)},
            {"6192", ("Sistema operativo Linux", 50, 4.50)},
            {"6193", ("Serviços de rede Linux", 25, 2.25)},
            {"6139", ("Redes de acesso", 25, 2.25)},
            {
                "6140",
                ("Redes de comunicações - encaminhamento dinâmico", 25,
                    2.25)
            },
            {"6141", ("Redes de comunicações - segurança", 25, 2.25)},
            {
                "6143",
                ("Redes de comunicações - arquitetura e construção da rede de distribuição",
                    50, 4.50)
            },
            {
                "6144",
                ("Redes de comunicações - deteção de avarias e equipamentos de rede",
                    50, 4.50)
            },
            {
                "6142",
                ("Redes de comunicações - protocolos de redes de distribuição",
                    25, 2.25)
            }
        };
}