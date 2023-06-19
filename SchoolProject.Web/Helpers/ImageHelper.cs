namespace SchoolProject.Web.Helpers;

public class ImageHelper : IImageHelper
{
    public async Task<string> UploadImageAsync(
        IFormFile? imageFile, string folder)
    {
        // Cria o diretório se não existir
        var folderPath = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "images", folder);
        Directory.CreateDirectory(folderPath);
        Directory.Exists(folderPath);


        // Cria o arquivo
        string filePath;
        string fileName;
        do
        {
            var guid = Guid.NewGuid().ToString();
            fileName = guid + ".jpg";

            filePath = Directory.GetCurrentDirectory() +
                       $"\\wwwroot\\images\\{folder}\\{fileName}";

            // Cria o diretório se não existir
            // Directory.CreateDirectory(filePath);
            // Path.Exists(filePath);
        } while (File.Exists(filePath));


        await using var stream =
            new FileStream(
                filePath, FileMode.Create, FileAccess.ReadWrite);


        if (imageFile != null) await imageFile.CopyToAsync(stream);


        return $"~/images/{folder}/{fileName}";
    }
}