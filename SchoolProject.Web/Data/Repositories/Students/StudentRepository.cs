using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Students;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Students.IStudentRepository" />
public class StudentRepository : GenericRepository<Student>, IStudentRepository
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
    public StudentRepository(
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
    public IOrderedQueryable<Student> GetStudents()
    {
        return _dataContext.Students
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
    public IOrderedQueryable<Student> GetStudentById(int id)
    {
        return _dataContext.Students
            .Include(s => s.CountryOfNationality)
            // .ThenInclude(c => c.Nationality)
            // .Include(s => s.CountryOfNationality)
            // .ThenInclude(c => c.CreatedBy)
            .Include(s => s.Birthplace)
            // .Include(s => s.Birthplace)
            // .ThenInclude(c => c.Nationality)
            // .Include(s => s.Birthplace)
            // .ThenInclude(c => c.CreatedBy)
            .Include(s => s.City)
            // .ThenInclude(c => c.CreatedBy)
            // .Include(s => s.City)
            // .ThenInclude(c => c.UpdatedBy)
            .Include(s => s.Gender)
            // .ThenInclude(c => c.CreatedBy)
            .Include(s => s.Gender)
            // .ThenInclude(c => c.UpdatedBy)
            .Include(s => s.AppUser)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            // Se desejar carregar as turmas associadas
            .Include(s => s.CourseStudents)
            .ThenInclude(scs => scs.Course)
            .ThenInclude(sc => sc.Enrollments)

            // Se desejar carregar os cursos associados
            // E seus detalhes, se necessário
            // .Include(t => t.StudentDisciplines)
            // .ThenInclude(tc => tc.Discipline)
            // .Include(s => s.Enrollments)
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Discipline)
            .Where(i => i.Id == id)
            .OrderByDescending(o => o.FirstName);
    }
}