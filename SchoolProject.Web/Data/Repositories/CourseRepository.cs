using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(DataContextMsSql dataContext) : base(dataContext)
    {
    }
}