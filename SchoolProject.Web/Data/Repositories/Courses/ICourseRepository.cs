using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc />
public interface ICourseRepository : IGenericRepository<Course>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Course> GetCourses();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IQueryable<Course> GetCourseByIdAsync(int id);


    /// <summary>
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    IOrderedQueryable<Course> GetCourseAsync(Course course);


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddCourseAsync(Course model);
}