using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Models.SchoolClasses;

public class SchoolClassCoursesPagesViewModel
{
    public required IQueryable<CourseDisciplines> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}