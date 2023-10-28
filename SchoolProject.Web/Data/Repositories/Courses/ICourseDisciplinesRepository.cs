using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc />
public interface
    ICourseDisciplinesRepository : IGenericRepository<CourseDiscipline>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<CourseDiscipline> GetCourseDisciplines();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<CourseDiscipline> GetCourseDisciplineById(int id);


    /// <summary>
    ///
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    IOrderedQueryable<CourseDiscipline> GetCourseDisciplineByGuid(Guid idGuid);
}