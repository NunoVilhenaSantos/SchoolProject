using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Models.Teachers;

/// <summary>
///     TeacherViewModel class.
/// </summary>
public class TeacherViewModel : Teacher
{
    /// <summary>
    ///     ImageFile property.
    /// </summary>
    [DisplayName("Image")]
    public new IFormFile? ImageFile { get; set; }
}