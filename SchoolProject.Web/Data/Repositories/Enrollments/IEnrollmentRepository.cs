using SchoolProject.Web.Data.Entities.Enrollments;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

/// <inheritdoc />
public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollments();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Enrollment> GetEnrollmentById(int id);
}