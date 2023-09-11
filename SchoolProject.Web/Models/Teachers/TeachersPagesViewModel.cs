using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Models.Teachers;

/// <summary>
///
/// </summary>
public class TeachersPagesViewModel
{
    public required IQueryable<Teacher> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}