using System.ComponentModel;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Models.SchoolClasses;

/// <summary>
///     Represents the view model for the school class.
/// </summary>
public class SchoolClassViewModel : SchoolClass
{
    /// <summary>
    ///     Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")]
    public new IFormFile? ImageFile { get; set; }
}