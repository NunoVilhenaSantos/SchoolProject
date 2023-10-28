using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models.Enrollments;

// public class ProductViewModel : Product
/// <summary>
///     The student view model.
/// </summary>
public class ProductViewModel : Student
{
    /// <summary>
    ///     Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")]
    public new required IFormFile ImageFile { get; set; }
}