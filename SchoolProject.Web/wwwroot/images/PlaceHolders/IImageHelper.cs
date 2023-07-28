namespace SchoolProject.Web.Helpers.Images;

public interface IImageHelper
{
    Task<string> UploadImageAsync(IFormFile? imageFile, string folder);
}