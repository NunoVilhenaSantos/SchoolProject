using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models;

// public class ProductViewModel : Product
/// <summary>
///    The student view model.
/// </summary>
public class ProductViewModel : Student
{
    /// <summary>
    ///   Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")] public new IFormFile ImageFile { get; set; }
}