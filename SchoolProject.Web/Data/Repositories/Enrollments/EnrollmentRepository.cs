using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

/// <inheritdoc />
public class EnrollmentRepository : GenericRepository<Enrollment>,
    IEnrollmentRepository
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
    public EnrollmentRepository(
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
    public IOrderedQueryable<Enrollment> GetEnrollments()
    {
        return _dataContext.Enrollments
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            // .Include(s => s.Enrollments)
            // .Include(s => s.CreatedBy)
            // .Include(s => s.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Enrollment> GetEnrollmentById(int id)
    {
        return _dataContext.Enrollments
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            // .Include(s => s.Enrollments)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }
}