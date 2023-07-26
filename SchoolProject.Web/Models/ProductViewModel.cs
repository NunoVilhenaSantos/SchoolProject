using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models;

// public class ProductViewModel : Product
public class ProductViewModel : Data.Entities.Students.Student
{
    [DisplayName("Image")] public IFormFile ImageFile { get; set; }
}