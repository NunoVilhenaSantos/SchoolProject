using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Courses.ICourseDisciplinesRepository" />
public class CourseDisciplinesRepository
    : GenericRepository<CourseDiscipline>, ICourseDisciplinesRepository
{
    // authenticated user
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // data context
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    // helpers
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <inheritdoc />
    public CourseDisciplinesRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite,
        AuthenticatedUserInApp authenticatedUserInApp,
        IStorageHelper storageHelper, IUserHelper userHelper) : base(
        dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
        _authenticatedUserInApp = authenticatedUserInApp;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
    }


    /// <inheritdoc />
    public IOrderedQueryable<CourseDiscipline> GetCourseDisciplines()
    {
        return _dataContext.CourseDisciplines
            .Include(cd => cd.Discipline)
            .Include(cd => cd.Course)
            // .Include(cd => cd.CreatedBy)
            // .Include(cd => cd.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<CourseDiscipline> GetCourseDisciplineById(int id)
    {
        return _dataContext.CourseDisciplines
            .Include(s => s.Discipline)
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<CourseDiscipline> GetCourseDisciplineByGuid(
        Guid idGuid)
    {
        return _dataContext.CourseDisciplines
            .Include(s => s.Discipline)
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.IdGuid == idGuid)
            .OrderByDescending(o => o.Id);
    }
}