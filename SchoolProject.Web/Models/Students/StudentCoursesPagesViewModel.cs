using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models.Students
{
    public class StudentCoursesPagesViewModel
    {

        public required IQueryable<StudentCourse> Records { get; set; }


        public required int PageNumber { get; set; }


        public required int PageSize { get; set; }


        public required int TotalCount { get; set; }

    }
}
