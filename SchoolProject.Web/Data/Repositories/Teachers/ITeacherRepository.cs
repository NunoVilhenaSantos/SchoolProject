using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Data.Repositories.Teachers;

/// <inheritdoc />
public interface ITeacherRepository : IGenericRepository<Teacher>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IOrderedQueryable<Teacher> GetTeachers();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IOrderedQueryable<Teacher> GetTeacherById(int id);
}