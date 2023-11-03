using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Enrollments.IEnrollmentRepository" />
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
            .OrderByDescending(o => o.StudentId);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Enrollment> GetEnrollmentByIdGuid(Guid idGuid)
    {
        return _dataContext.Enrollments
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            // .Include(s => s.Enrollments)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.IdGuid == idGuid)
            .OrderByDescending(o => o.StudentId);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Enrollment> GetEnrollmentByEmail(string email)
    {
        return _dataContext.Enrollments
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            .ThenInclude(s => s.AppUser)
            // .Include(s => s.Enrollments)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.Student.AppUser.Email == email)
            .OrderByDescending(o => o.StudentId);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Enrollment> GetEnrollmentsByStudentId(
        int studentId)
    {
        return _dataContext.Enrollments
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            .ThenInclude(s => s.AppUser)
            // .Include(s => s.Enrollments)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .Where(i => i.StudentId == studentId)
            .OrderByDescending(o => o.StudentId);
    }


    /// <inheritdoc />
    public Task DeleteStudentByIdAsync(
        int studentId, HashSet<CourseDiscipline>? courseDisciplines)
    {
        // var studentEnrollments =
        //     GetEnrollmentsByStudentId(studentId).ToList();

        var disciplines =
            courseDisciplines?.Select(s => s.Discipline).ToList();


        var studentDisciplines = _dataContext.StudentDisciplines
            .Include(s => s.Discipline)
            .Include(s => s.Student)
            .Where(sd => sd.StudentId == studentId)
            .Where(sd => disciplines != null && disciplines.Contains(sd.Discipline))
            .ToList();


        var studentEnrollments = GetEnrollmentsByStudentId(studentId)
            .Where(enrollment => disciplines != null && disciplines.Contains(enrollment.Discipline))
            .ToList();


        _dataContext.Enrollments.RemoveRange(studentEnrollments);

        _dataContext.StudentDisciplines.RemoveRange(studentDisciplines);

        return _dataContext.SaveChangesAsync();

        // return Task.CompletedTask;
    }



    /// <inheritdoc />
    public Task DeleteRangeAsync(List<Enrollment> enrollments)
    {
        _dataContext.Enrollments.RemoveRange(enrollments);

        return _dataContext.SaveChangesAsync();

        // return Task.CompletedTask;
    }
}