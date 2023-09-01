using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Models.Teachers;

public class TeacherViewModel : Teacher
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}