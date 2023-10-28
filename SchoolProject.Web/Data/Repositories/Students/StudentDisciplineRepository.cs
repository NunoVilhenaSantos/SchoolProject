using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Students;

/// <inheritdoc />
public class StudentDisciplineRepository
    : GenericRepository<StudentDiscipline>, IStudentDisciplineRepository
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
    public StudentDisciplineRepository(
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
    public IOrderedQueryable<StudentDiscipline> GetStudentDisciplines()
    {
        return _dataContext.StudentDisciplines
            .Include(scc => scc.Student)
            .Include(scc => scc.Discipline)
            // .Include(scc => scc.CreatedBy)
            // .Include(scc => scc.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderBy(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<StudentDiscipline>
        GetStudentDisciplineById(int id)
    {
        return _dataContext.StudentDisciplines
            .Include(scc => scc.Student)
            .Include(scc => scc.Discipline)
            .Include(scc => scc.CreatedBy)
            .Include(scc => scc.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderBy(o => o.Id);
    }
}