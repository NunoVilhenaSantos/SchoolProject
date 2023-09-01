using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Models.Courses;

public class CoursesPagesViewModel
{
    public required IQueryable<Course> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}