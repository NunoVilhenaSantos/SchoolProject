using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models.Students;

public class StudentsPagesViewModel
{
    public required IQueryable<Student> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}