using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Disciplines;

/// <inheritdoc />
public class DisciplineRepository : GenericRepository<Discipline>,
    IDisciplineRepository
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
    public DisciplineRepository(
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
    public IOrderedQueryable<Discipline> GetDisciplines()
    {
        return _dataContext.Disciplines
            .OrderBy(o => o.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Discipline> GetDisciplinesList()
    {
        return _dataContext.Disciplines
            .Include(s => s.CourseDisciplines)
            .Include(s => s.StudentDisciplines)
            .Include(s => s.TeacherDisciplines)
            .Include(s => s.Enrollments)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderByDescending(o => o.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Discipline> GetDisciplineById(int id)
    {
        return _dataContext.Disciplines
            .Include(s => s.CourseDisciplines)
            .ThenInclude(s => s.Course)
            .Include(s => s.StudentDisciplines)
            .ThenInclude(s => s.Student)
            .Include(s => s.TeacherDisciplines)
            .ThenInclude(s => s.Teacher)
            .Include(s => s.Enrollments)
            .ThenInclude(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.Name);
    }
}