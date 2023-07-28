namespace SchoolProject.Web.Data.Seeders;

public class SeedDbPlaceHolders
{
    //
    // Injeção de dependência do IWebHostEnvironment
    // para obter o diretorio de execução do projeto
    //
    private static IWebHostEnvironment _hostingEnvironment;


    // Add an initializer to receive IUserHelper through dependency injection
    public static void Initialize(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }


    internal static void AddPlaceHolders()
    {
        var baseDirectory = _hostingEnvironment.ContentRootPath;
        var diretorioBase =
            Path.GetFullPath(Path.Combine(baseDirectory, "Helpers/Images"));


        var origem = Path.Combine(_hostingEnvironment.ContentRootPath,
            "Helpers", "Images");
        var destino = Path.Combine(_hostingEnvironment.WebRootPath,
            "images", "PlaceHolders");


        // Cria o diretório se não existir
        var folderPath = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "images",
            "PlaceHolders");
        Directory.CreateDirectory(destino);
        Directory.Exists(destino);


        // Obtém todos os caminhos dos arquivos na pasta de origem
        var arquivos = Directory.GetFiles(origem);


        // Itera sobre os caminhos dos arquivos e copia cada um para a pasta de destino
        foreach (var arquivo in arquivos)
        {
            var nomeArquivo = Path.GetFileName(arquivo);
            var caminhoDestino = Path.Combine(destino, nomeArquivo);
            File.Copy(arquivo, caminhoDestino, true);
        }

        Console.WriteLine("Placeholders adicionados com sucesso!");
    }
}