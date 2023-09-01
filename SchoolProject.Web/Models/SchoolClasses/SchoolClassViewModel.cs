using System.ComponentModel;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Models.SchoolClasses;

public class SchoolClassViewModel : SchoolClass
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}