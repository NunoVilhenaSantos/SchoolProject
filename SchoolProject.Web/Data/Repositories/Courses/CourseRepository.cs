using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Data.Repositories.Courses;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Courses.ICourseRepository" />
public class CourseRepository
    : GenericRepository<Course>, ICourseRepository
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
    public CourseRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite,
        AuthenticatedUserInApp authenticatedUserInApp,
        IStorageHelper storageHelper, IUserHelper userHelper) : base(
        dataContext,
        dataContextMySql, dataContextMsSql, dataContextSqLite)
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
    public IOrderedQueryable<Course> GetCourses()
    {
        return _dataContext.Courses
            .OrderBy(o => o.Name);
    }


    /// <inheritdoc />
    public IQueryable<Course> GetCourseByIdAsync(int id)
    {
        return _dataContext.Courses
            // ----- (Enrollments) Students / Disciplines ------------------ //
            .Include(i => i.Enrollments)
            .ThenInclude(i => i.Student)
            .Include(i => i.Enrollments)
            .ThenInclude(i => i.Discipline)

            // ------------------- Courses / Disciplines ------------------- //
            .Include(i => i.CourseDisciplines)
            .ThenInclude(i => i.Discipline)

            // ------------------- Courses / Students --------------------- //
            .Include(i => i.CourseStudents)
            .ThenInclude(i => i.Student)
            // .Include(i => i.CourseStudents)
            .Include(i => i.CreatedBy)
            .Include(i => i.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Course> GetCourseAsync(Course course)
    {
        return _dataContext.Courses
            // ----- (Enrollments) Students / Disciplines ------------------ //
            .Include(i => i.Enrollments)
            .ThenInclude(i => i.Student)
            .Include(i => i.Enrollments)
            .ThenInclude(i => i.Discipline)

            // ------------------- Courses / Disciplines ------------------- //
            .Include(i => i.CourseDisciplines)
            .ThenInclude(i => i.Discipline)

            // ------------------- Courses / Students --------------------- //
            .Include(i => i.CourseStudents)
            .ThenInclude(i => i.Student)
            // .Include(i => i.CourseStudents)
            .Include(i => i.CreatedBy)
            .Include(i => i.UpdatedBy)
            .Where(i => i.Id == course.Id)
            .OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public async Task AddCourseAsync(Course model)
    {
        var course = await GetCourseAsync(model).FirstOrDefaultAsync();

        if (course == null) return;

        course = new Course
        {
            Name = model.Name,
            Code = model.Code, Acronym = model.Acronym,
            QnqLevel = model.QnqLevel, EqfLevel = model.EqfLevel,
            StartDate = model.StartDate, EndDate = model.EndDate,
            StartHour = model.StartHour, EndHour = model.EndHour,
            PriceForEmployed = model.PriceForEmployed,
            PriceForUnemployed = model.PriceForUnemployed,
            ProfilePhotoId = model.ProfilePhotoId,
            WasDeleted = model.WasDeleted,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
        };

        _dataContext.Courses.Update(course);

        await _dataContext.SaveChangesAsync();
    }
}