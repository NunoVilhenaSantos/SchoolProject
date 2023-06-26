using System.ComponentModel;
using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Models;

public class TeacherViewModel : Teacher
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}