using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Teachers;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Teachers.ITeacherRepository" />
public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
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
    public TeacherRepository(
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
    public IOrderedQueryable<Teacher> GetTeachers()
    {
        return _dataContext.Teachers
            // .Include(t => t.Birthplace)
            // .Include(t => t.City)
            // .Include(t => t.CountryOfNationality)
            // .Include(t => t.Gender)
            // .Include(t => t.CreatedBy)
            // .Include(t => t.UpdatedBy)
            // .Include(t => t.AppUser)
            // .Include(s => s.TeacherDisciplines)
            // .Where(i => i.Id == id)
            .OrderByDescending(o => o.FirstName);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Teacher> GetTeacherById(int id)
    {
        return _dataContext.Teachers
            .Include(t => t.Birthplace)
            .Include(t => t.City)
            .Include(t => t.CountryOfNationality)
            .Include(t => t.Gender)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .Include(t => t.AppUser)
            .Include(s => s.TeacherDisciplines)
            .ThenInclude(e => e.Discipline)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.FirstName);
    }
}