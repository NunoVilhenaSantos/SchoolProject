using SchoolProject.Web.Data.Entities.Enrollments;

namespace SchoolProject.Web.Models.Enrollments;

public class EnrollmentsPagesViewModel
{
    public required IQueryable<Enrollment> Records { get; set; }


    public required int PageNumber { get; set; }


    public required int PageSize { get; set; }


    public required int TotalCount { get; set; }
}