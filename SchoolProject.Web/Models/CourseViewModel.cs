using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Models;

public class CourseViewModel : Course
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}