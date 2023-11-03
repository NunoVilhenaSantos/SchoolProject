using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Data.Repositories.Enrollments;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Courses;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Courses and Students Controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesStudentsController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(CourseStudent.CourseId);
    internal const string CurrentClass = nameof(CourseStudent);

    internal const string CurrentAction = nameof(IndexCards1);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly ICourseRepository _courseRepository;


    //  repositories
    private readonly ICourseStudentsRepository _courseStudentsRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserHelper _userHelper;
    private readonly UserManager<AppUser> _userManager;


    // database context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     SchoolClassStudentsController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="courseStudentsRepository"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="courseRepository"></param>
    /// <param name="studentRepository"></param>
    /// <param name="userManager"></param>
    public CoursesStudentsController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        AuthenticatedUserInApp authenticatedUserInApp,
        ICourseStudentsRepository courseStudentsRepository,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        ICourseRepository courseRepository,
        IStudentRepository studentRepository, UserManager<AppUser> userManager,
        IEnrollmentRepository enrollmentRepository)
    {
        _courseStudentsRepository = courseStudentsRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _studentRepository = studentRepository;
        _userManager = userManager;
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        // _context = context;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CoursesStudentsController));


    // Obtém o controlador atual
    private string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }


    /// <summary>
    ///     Course Student Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CourseStudentNotFound()
    {
        return View();
    }


    private List<CourseStudentViewModel> GetCoursesAndStudentsList()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var courseStudentList =
            _courseStudentsRepository.GetCourseStudents()
                .Include(e => e.CreatedBy)
                .Include(e => e.UpdatedBy)
                .AsNoTracking()
                .ToList();

        var viewModelList = courseStudentList.Select(item =>
            new CourseStudentViewModel
            {
                Id = item.Id,
                IdGuid = item.IdGuid,
                CourseId = item.Course.Id,
                CourseIdGuid = item.Course.IdGuid,
                CourseCode = item.Course.Code,
                CourseAcronym = item.Course.Acronym,
                CourseName = item.Course.Name,
                StudentId = item.Student.Id,
                StudentIdGuid = item.Student.IdGuid,
                StudentFullName = item.Student.FullName,
                StudentMobilePhone = item.Student.MobilePhone,
                WasDeleted = item.WasDeleted,
                CreatedAt = item.CreatedAt,
                CreatedByFullName = item.CreatedBy.FullName,
                UpdatedAt = item.UpdatedAt,
                UpdatedByFullName = item.UpdatedBy?.FullName ?? string.Empty
            }).ToList();


        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine(
            "Tempo de execução: GetSchoolClassesAndCourses " +
            $"{elapsedMilliseconds} ms");


        // return courseStudentList;
        return viewModelList;
    }


    private List<CourseStudentViewModel> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<CourseStudentViewModel> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<CourseStudentViewModel>>(
                    json) ??
                new List<CourseStudentViewModel>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            recordsQuery = GetCoursesAndStudentsList();

            // Inicialize a classe de paginação
            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<CourseStudentViewModel>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: CourseStudents
    /// <summary>
    ///     Index, list all SchoolClassStudents
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<CourseStudentViewModel>();

        return View(recordsQuery);
    }


    // GET: CourseStudents
    /// <summary>
    ///     IndexCards, list all SchoolClassStudents
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<CourseStudentViewModel>();

        return View(recordsQuery);
    }


    // GET: CourseStudents
    /// <summary>
    ///     IndexCards1, list all SchoolClassStudents
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<CourseStudentViewModel>();

        var model = new PaginationViewModel<CourseStudentViewModel>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: CourseStudents/Details/5
    /// <summary>
    ///     Details, details of a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        var courseStudent = id != 0 || idGuid == Guid.Empty
            ? await _courseStudentsRepository
                .GetCourseStudentById(id.Value)
                .FirstOrDefaultAsync()
            : await _courseStudentsRepository
                .GetCourseStudentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        return courseStudent == null
            ? new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1))
            : View(courseStudent);
    }


    // GET: CourseStudents/Create
    /// <summary>
    ///     Create, create a new courseStudent
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        var courseStudent = new CourseStudent
        {
            CourseId = 0, Course = null, Student = null, StudentId = 0,
            CreatedBy = null, CreatedById = null, CreatedAt = DateTime.Now
        };

        FillViewLists();

        return View(courseStudent);
    }


    // POST: CourseStudents/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, create a new courseStudent
    /// </summary>
    /// <param name="courseStudent"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CourseStudent courseStudent)
    {
        var student = await _studentRepository
            .GetByIdAsync(courseStudent.StudentId)
            .FirstOrDefaultAsync();

        var course = await _courseRepository
            .GetByIdAsync(courseStudent.CourseId)
            .Include(o => o.CourseDisciplines)
            .ThenInclude(o => o.Discipline)
            .FirstOrDefaultAsync();

        var user = await _authenticatedUserInApp.GetAuthenticatedUser();

        var courseStudent1 = new CourseStudent
        {
            CreatedBy = user, Course = course, Student = student
        };


        try
        {
            await _courseStudentsRepository.CreateAsync(courseStudent1);

            foreach (var enrollment in course.CourseDisciplines.Select(
                         disciplines => new Enrollment
                         {
                             Student = student,
                             Discipline = disciplines.Discipline,
                             CreatedBy = user
                         })
                    )
                try
                {
                    await _enrollmentRepository.CreateAsync(enrollment);
                }
                catch (Exception e)
                {
                    var message = $"This student {student.FullName} " +
                                  $"is already enrool in this course {course.Name}.";

                    ModelState.AddModelError(string.Empty,
                        message);

                    Console.WriteLine(message + "\n" + e);
                }

            await _courseStudentsRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(IndexCards1));
        }
        catch (Exception e)
        {
            FillViewLists(
                courseStudent1.CourseId, courseStudent1.StudentId,
                courseStudent1.CreatedBy?.Id, courseStudent1.UpdatedBy?.Id);

            ModelState.AddModelError(string.Empty,
                $"This student {student.FullName} " +
                $"is already enrool in this course {course.Name}.");

            return View(courseStudent1);
        }
    }


    // GET: CourseStudents/Edit/5
    /// <summary>
    ///     Edit, edit a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        var courseStudent = id != 0 || idGuid == Guid.Empty
            ? await _courseStudentsRepository
                .GetCourseStudentById(id.Value)
                .FirstOrDefaultAsync()
            : await _courseStudentsRepository
                .GetCourseStudentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        if (courseStudent == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));

        FillViewLists();

        return View(courseStudent);
    }


    // POST: CourseStudents/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, edit a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <param name="courseStudent"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, Guid idGuid, CourseStudent courseStudent)
    {
        if (id != courseStudent.Id || idGuid != courseStudent.IdGuid)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        if (!ModelState.IsValid)
        {
            FillViewLists(
                courseStudent.CourseId, courseStudent.StudentId,
                courseStudent.CreatedById, courseStudent.UpdatedById);

            return View(courseStudent);
        }


        try
        {
            await _courseStudentsRepository.UpdateAsync(courseStudent);

            await _courseStudentsRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(IndexCards1));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseStudentsExists(courseStudent.CourseId))
                return new NotFoundViewResult(
                    nameof(CourseStudentNotFound), CurrentClass,
                    id.ToString(), CurrentController, nameof(IndexCards1));

            throw;
        }
    }


    // GET: CourseStudents/Delete/5
    /// <summary>
    ///     Delete, delete a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        var courseStudent = id != 0 || idGuid == Guid.Empty
            ? await _courseStudentsRepository
                .GetCourseStudentById(id.Value)
                .FirstOrDefaultAsync()
            : await _courseStudentsRepository
                .GetCourseStudentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        return courseStudent == null
            ? new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1))
            : View(courseStudent);
    }


    // POST: CourseStudents/Delete/5
    /// <summary>
    ///     DeleteConfirmed, delete a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, Guid idGuid)
    {
        var courseStudent = id != 0 || idGuid == Guid.Empty
            ? await _courseStudentsRepository
                .GetCourseStudentById(id)
                .FirstOrDefaultAsync()
            : await _courseStudentsRepository
                .GetCourseStudentByIdGuid(idGuid)
                .FirstOrDefaultAsync();


        if (courseStudent == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        var studentEnrollments = _enrollmentRepository
            .GetEnrollmentsByStudentId(courseStudent.StudentId)
            .ToList();


        var course = await _courseRepository
            .GetByIdAsync(courseStudent.CourseId)
            .Include(o => o.CourseDisciplines)
            .ThenInclude(o => o.Discipline)
            .FirstOrDefaultAsync();


        try
        {
            await _courseStudentsRepository.DeleteAsync(courseStudent);

            await _courseStudentsRepository.SaveAllAsync();


            await _enrollmentRepository.DeleteStudentByIdAsync(
                courseStudent.StudentId, course.CourseDisciplines);

            await _enrollmentRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(IndexCards1));
        }
        catch (DbUpdateException ex)
        {
            // Handle DbUpdateException, specifically for this controller.
            Console.WriteLine(ex.Message);

            // Handle foreign key constraint violation.
            DbErrorViewModel dbErrorViewModel;

            if (ex.InnerException != null &&
                ex.InnerException.Message.Contains("DELETE"))
            {
                dbErrorViewModel = new DbErrorViewModel
                {
                    DbUpdateException = true,
                    ErrorTitle = "Foreign Key Constraint Violation",
                    ErrorMessage =
                        "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                        $"The {nameof(CourseStudent)} with the ID " +
                        $"{courseStudent.Id} - " +
                        $"{courseStudent.Course.Name} " +
                        $"{courseStudent.IdGuid} " +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(CourseStudent),
                    ItemId = courseStudent.Id.ToString(),
                    ItemGuid = courseStudent.IdGuid,
                    ItemName = courseStudent.Course.Name
                };

                // Redirecione para o DatabaseError com os dados apropriados
                return RedirectToAction(
                    "DatabaseError", "Errors", dbErrorViewModel);
            }

            // Handle other DbUpdateExceptions.
            dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Database Error",
                ErrorMessage = "An error occurred while deleting the entity.",
                ItemClass = nameof(CourseStudent),
                ItemId = courseStudent.Id.ToString(),
                ItemGuid = courseStudent.IdGuid,
                ItemName = courseStudent.Course.Name
            };

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (InvalidOperationException ex)
        {
            // Handle the exception
            Console.WriteLine(
                "An InvalidOperationException occurred: " + ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(CourseStudent)} with the ID " +
                    $"{courseStudent.Id} - {courseStudent.Student.FullName} " +
                    $"{courseStudent.IdGuid} " +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(CourseStudent),
                ItemId = courseStudent.Id.ToString(),
                ItemGuid = courseStudent.IdGuid,
                ItemName = courseStudent.Student.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (Exception ex)
        {
            // Catch any other exceptions that might occur
            Console.WriteLine("An error occurred: " + ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(CourseStudent)} with the ID " +
                    $"{courseStudent.Id} - {courseStudent.Student.FullName} " +
                    $"{courseStudent.IdGuid}" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(CourseStudent),
                ItemId = courseStudent.Id.ToString(),
                ItemGuid = courseStudent.IdGuid,
                ItemName = courseStudent.Student.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private bool CourseStudentsExists(int id)
    {
        return _courseStudentsRepository.ExistAsync(id).Result;
    }


    private void FillViewLists(
        int courseId = 0, int studentId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        var loggedUser = _authenticatedUserInApp.GetAuthenticatedUser().Result;


        ViewData[nameof(CourseDiscipline.CourseId)] =
            new SelectList(
                _courseRepository.GetCourses().Select(c =>
                        new {c.Id, Description = $"{c.Code} - {c.Name}"})
                    .ToList(),
                nameof(Course.Id),
                "Description",
                courseId);


        ViewData[nameof(CourseStudent.StudentId)] =
            new SelectList(_studentRepository.GetAll().ToList(),
                // _studentRepository.GetAll().Select(c => new { Id = c.Id, Description = $"{c.FirstName} - {c.LastName}" }).ToList(),
                nameof(Student.Id),
                nameof(Student.FullName),
                // "Description",
                studentId);


        ViewData[nameof(CourseDiscipline.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                createdById != null ? createdById : loggedUser.Id);

        ViewData[nameof(CourseDiscipline.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                updatedById != null ? updatedById : loggedUser.Id);
    }
}