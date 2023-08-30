using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Models.Teachers
{
    public class TeacherCoursesPagesViewModel
    {

        public required IQueryable<TeacherCourse> Records { get; set; }


        public required int PageNumber { get; set; }


        public required int PageSize { get; set; }


        public required int TotalCount { get; set; }

    }
}
