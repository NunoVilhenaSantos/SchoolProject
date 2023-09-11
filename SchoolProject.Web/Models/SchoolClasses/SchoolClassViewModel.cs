using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Models.SchoolClasses;

/// <summary>
///     Represents the view model for the school class.
/// </summary>
public class SchoolClassViewModel : Course
{
    /// <summary>
    ///     Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")]
    public new IFormFile? ImageFile { get; set; }
}