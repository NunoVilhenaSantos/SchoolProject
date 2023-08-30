using System.ComponentModel;

namespace SchoolProject.Web.Models.SchoolClasses;

public class SchoolClassViewModel : Data.Entities.SchoolClasses.SchoolClass
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}