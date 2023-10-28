using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Repositories.Students;

/// <inheritdoc />
public interface IStudentRepository : IGenericRepository<Student>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Student> GetStudents();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Student> GetStudentById(int id);
}