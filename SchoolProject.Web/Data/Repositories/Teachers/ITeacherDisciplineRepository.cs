using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Data.Repositories.Teachers;

/// <inheritdoc />
public interface
    ITeacherDisciplineRepository : IGenericRepository<TeacherDiscipline>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IOrderedQueryable<TeacherDiscipline> GetTeacherDisciplines();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<TeacherDiscipline> GetTeacherDisciplinesById(int id);
}