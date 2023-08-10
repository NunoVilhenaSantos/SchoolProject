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
        var origem =
            Path.Combine(path1: _hostingEnvironment.ContentRootPath,
                path2: "Helpers", path3: "Images");
        var destino =
            Path.Combine(path1: _hostingEnvironment.WebRootPath,
                path2: "images", path3: "PlaceHolders");


        // Cria o diretório de destino se não existir
        Directory.CreateDirectory(path: destino);

        // Obtém todos os caminhos dos arquivos na pasta de origem
        var arquivos = Directory.GetFiles(path: origem);

        // Itera sobre os caminhos dos arquivos e
        // copia cada um para a pasta de destino
        foreach (var arquivo in arquivos)
        {
            var nomeArquivo = Path.GetFileName(path: arquivo);
            var extensao = Path.GetExtension(path: arquivo);

            // Verifica se a extensão do arquivo não é
            // .cs (arquivo C#) antes de copiá-lo
            if (extensao == ".cs") continue;

            var caminhoDestino = Path.Combine(path1: destino, path2: nomeArquivo);
            File.Copy(sourceFileName: arquivo, destFileName: caminhoDestino, overwrite: true);
        }

        Console.WriteLine(value: "Placeholders adicionados com sucesso!");
    }
}