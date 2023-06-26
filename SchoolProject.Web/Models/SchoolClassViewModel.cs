using System.ComponentModel;
using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Models;

public class SchoolClassViewModel : SchoolClass
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}