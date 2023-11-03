using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Repositories.Students;

/// <inheritdoc />
public interface
    IStudentDisciplineRepository : IGenericRepository<StudentDiscipline>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<StudentDiscipline> GetStudentDisciplines();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<StudentDiscipline> GetStudentDisciplineById(int id);


    /// <summary>
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    IOrderedQueryable<StudentDiscipline>
        GetStudentDisciplineByIdGuid(Guid idGuid);
    Task DeleteRangeAsync(List<StudentDiscipline> studentDisciplines);
}