using System.ComponentModel;

namespace SchoolProject.Web.Models.Course;

public class CourseViewModel : Data.Entities.Courses.Course
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}