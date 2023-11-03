using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

/// <inheritdoc />
public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollments();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollmentById(int id);


    /// <summary>
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollmentByIdGuid(Guid idGuid);


    /// <summary>
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollmentByEmail(string email);


    /// <summary>
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollmentsByStudentId(int studentId);

    /// <summary>
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseDisciplines"></param>
    /// <returns></returns>
    Task DeleteStudentByIdAsync(
        int studentId, HashSet<CourseDiscipline>? courseDisciplines);


    Task DeleteRangeAsync(List<Enrollment> enrollments);
}