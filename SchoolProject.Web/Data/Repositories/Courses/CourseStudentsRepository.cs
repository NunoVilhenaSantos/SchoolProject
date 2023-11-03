using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Courses.ICourseStudentsRepository" />
public class CourseStudentsRepository
    : GenericRepository<CourseStudent>, ICourseStudentsRepository
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
    public CourseStudentsRepository(
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
    public IOrderedQueryable<CourseStudent> GetCourseStudents()
    {
        return _dataContext.CourseStudents
            .Include(cd => cd.Student)
            .Include(cd => cd.Course)
            // .Include(cd => cd.CreatedBy)
            // .Include(cd => cd.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<CourseStudent> GetCourseStudentById(int id)
    {
        return _dataContext.CourseStudents

            // --------------- Course section ---------------------- //
            .Include(cs => cs.Course)

            // --------------- Student section --------------------- //
            .Include(cs => cs.Student)
            //.ThenInclude(s => s.Country)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.CountryOfNationality)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.Birthplace)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.Gender)
            //.ThenInclude(g => g.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.AppUser)

            // --------------- Student Others section -------------- //
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.SchoolClassStudents)

            // --------------- Others section ---------------------- //
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<CourseStudent> GetCourseStudentByIdGuid(
        Guid idGuid)
    {
        return _dataContext.CourseStudents

            // --------------- Course section ---------------------- //
            .Include(cs => cs.Course)

            // --------------- Student section --------------------- //
            .Include(cs => cs.Student)
            //.ThenInclude(s => s.Country)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.CountryOfNationality)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.Birthplace)
            //.ThenInclude(c => c.Nationality)
            //.ThenInclude(n => n.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.Gender)
            //.ThenInclude(g => g.CreatedBy)
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.AppUser)

            // --------------- Student Others section -------------- //
            //.Include(scs => cs.Student)
            //.ThenInclude(s => s.SchoolClassStudents)

            // --------------- Others section ---------------------- //
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.IdGuid == idGuid)
            .OrderByDescending(o => o.StudentId);
    }
}