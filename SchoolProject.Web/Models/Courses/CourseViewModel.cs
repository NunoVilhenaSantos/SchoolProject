using System.ComponentModel;

namespace SchoolProject.Web.Models.Courses;

public class CourseViewModel : Data.Entities.Courses.Course
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}