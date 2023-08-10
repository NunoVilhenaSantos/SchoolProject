namespace SchoolProject.Web.Helpers.Images;

public class ImageHelper : IImageHelper
{
    public async Task<string> UploadImageAsync(
        IFormFile? imageFile, string folder)
    {
        // Cria o diretório se não existir
        var folderPath = Path.Combine(
            path1: Directory.GetCurrentDirectory(), path2: "wwwroot", path3: "images", path4: folder);
        Directory.CreateDirectory(path: folderPath);
        Directory.Exists(path: folderPath);


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
        } while (File.Exists(path: filePath));


        await using var stream =
            new FileStream(
                path: filePath, mode: FileMode.Create, access: FileAccess.ReadWrite);


        if (imageFile != null) await imageFile.CopyToAsync(target: stream);


        return $"~/images/{folder}/{fileName}";
    }
}