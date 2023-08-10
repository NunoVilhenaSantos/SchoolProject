using System.ComponentModel;

namespace SchoolProject.Web.Models.Course;

public class CourseViewModel : Data.Entities.Courses.Course
{
    [DisplayName(displayName: "Image")] public IFormFile? ImageFile { get; set; }
}