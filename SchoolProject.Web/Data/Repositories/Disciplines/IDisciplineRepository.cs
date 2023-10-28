using SchoolProject.Web.Data.Entities.Disciplines;

namespace SchoolProject.Web.Data.Repositories.Disciplines;

/// <inheritdoc />
public interface IDisciplineRepository : IGenericRepository<Discipline>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Discipline> GetDisciplines();


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Discipline> GetDisciplinesList();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Discipline> GetDisciplineById(int id);
}