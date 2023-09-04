using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Models.Courses;

/// <summary>
///     The course view model.
/// </summary>
public class CourseViewModel : Course
{
    /// <summary>
    ///     Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")]
    public new IFormFile? ImageFile { get; set; }
}