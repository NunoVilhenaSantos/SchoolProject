namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///
/// </summary>
public class SeedDbPlaceHolders
{
    //
    // Injeção de dependência do IWebHostEnvironment
    // para obter o diretório de execução do projeto
    //
    private static IWebHostEnvironment _webHostEnvironment;


    /// <summary>
    /// Add an initializer to receive IWebHostEnvironment hostingEnvironment
    /// </summary>
    /// <param name="webHostEnvironment"></param>
    public static void Initialize(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    internal static void AddPlaceHolders()
    {
        var origem =
            Path.Combine(_webHostEnvironment.ContentRootPath,
                "Helpers", "Images");
        var destino =
            Path.Combine(_webHostEnvironment.WebRootPath,
                "images", "PlaceHolders");


        // Cria o diretório de destino se não existir
        Directory.CreateDirectory(destino);

        // Obtém todos os caminhos dos arquivos na pasta de origem
        var arquivos = Directory.GetFiles(origem);

        // Itera sobre os caminhos dos arquivos e
        // copia cada um para a pasta de destino
        foreach (var arquivo in arquivos)
        {
            var nomeArquivo = Path.GetFileName(arquivo);
            var extensao = Path.GetExtension(arquivo);

            // Verifica se a extensão do arquivo não é
            // .cs (arquivo C#) antes de copiá-lo
            if (extensao == ".cs") continue;

            var caminhoDestino = Path.Combine(destino, nomeArquivo);
            File.Copy(arquivo, caminhoDestino, true);
        }

        Console.WriteLine("Placeholders adicionados com sucesso!");
    }
}