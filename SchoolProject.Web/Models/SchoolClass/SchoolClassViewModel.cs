using System.ComponentModel;

namespace SchoolProject.Web.Models.SchoolClass;

public class SchoolClassViewModel : Data.Entities.SchoolClasses.SchoolClass
{
    [DisplayName(displayName: "Image")] public IFormFile? ImageFile { get; set; }
}