using System.ComponentModel;

namespace SchoolProject.Web.Models.SchoolClass;

public class SchoolClassViewModel : Data.Entities.SchoolClasses.SchoolClass
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}