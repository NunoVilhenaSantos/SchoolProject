using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Disciplines;

namespace SchoolProject.Web.Models.Disciplines;

/// <summary>
///     The course view model.
/// </summary>
public class DisciplinesViewModel : Discipline
{
    /// <summary>
    ///     Gets or sets the image file.
    /// </summary>
    [DisplayName("Image")]
    public new IFormFile? ImageFile { get; set; }
}