using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc />
public interface ICourseStudentsRepository
    : IGenericRepository<CourseStudent>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<CourseStudent> GetCourseStudents();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<CourseStudent> GetCourseStudentById(int id);


    /// <summary>
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    IOrderedQueryable<CourseStudent> GetCourseStudentByIdGuid(Guid idGuid);
}