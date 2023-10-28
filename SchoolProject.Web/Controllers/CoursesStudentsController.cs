using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Courses and Students Controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesStudentsController : Controller
{
    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(CourseStudent.CourseId);
    internal const string CurrentClass = nameof(CourseStudent);
    internal const string CurrentAction = nameof(Index);


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CoursesStudentsController));


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMailHelper _mailHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;


    //  repositories
    private readonly ICourseStudentsRepository _courseStudentsRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;
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
        IStudentRepository studentRepository, UserManager<AppUser> userManager)
    {
        _courseStudentsRepository = courseStudentsRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _studentRepository = studentRepository;
        _userManager = userManager;
        _courseRepository = courseRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        // _context = context;
    }


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


    private List<CourseStudent> GetCoursesAndStudentsList()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var courseStudentList =
            _courseStudentsRepository.GetCourseStudents().ToList();

        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Tempo de execução: " +
                          "GetSchoolClassesAndCourses " +
                          $"{elapsedMilliseconds} ms");

        return courseStudentList;
    }


    private List<CourseStudent> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<CourseStudent> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<CourseStudent>>(json) ??
                new List<CourseStudent>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            recordsQuery = GetCoursesAndStudentsList();

            // Inicialize a classe de paginação
            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<CourseStudent>
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

        var recordsQuery = SessionData<CourseStudent>();

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

        var recordsQuery = SessionData<CourseStudent>();

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

        var recordsQuery = SessionData<CourseStudent>();

        var model = new PaginationViewModel<CourseStudent>(
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
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var courseStudent = await _courseStudentsRepository
            .GetCourseStudentById(id.Value).FirstOrDefaultAsync();

        return courseStudent == null
            ? new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(courseStudent);
    }


    // GET: CourseStudents/Create
    /// <summary>
    ///     Create, create a new courseStudent
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        FillViewLists();

        return View();
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
        if (ModelState.IsValid)
        {
            await _courseStudentsRepository.CreateAsync(courseStudent);

            await _courseStudentsRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }

        FillViewLists();

        return View(courseStudent);
    }


    // GET: CourseStudents/Edit/5
    /// <summary>
    ///     Edit, edit a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var courseStudent = await _courseStudentsRepository
            .GetCourseStudentById(id.Value).FirstOrDefaultAsync();

        if (courseStudent == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

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
    /// <param name="courseStudent"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, CourseStudent courseStudent)
    {
        if (id != courseStudent.CourseId)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        if (!ModelState.IsValid)
        {
            FillViewLists();

            return View(courseStudent);
        }

        try
        {
            await _courseStudentsRepository.UpdateAsync(courseStudent);

            await _courseStudentsRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SchoolClassStudentExists(courseStudent.CourseId))
                return new NotFoundViewResult(
                    nameof(CourseStudentNotFound), CurrentClass,
                    id.ToString(), CurrentController, nameof(Index));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: CourseStudents/Delete/5
    /// <summary>
    ///     Delete, delete a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var courseStudent = await _courseStudentsRepository
            .GetCourseStudentById(id.Value).FirstOrDefaultAsync();

        return courseStudent == null
            ? new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(courseStudent);
    }


    // POST: CourseStudents/Delete/5
    /// <summary>
    ///     DeleteConfirmed, delete a courseStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var courseStudent = await _courseStudentsRepository
            .GetCourseStudentById(id).FirstOrDefaultAsync();

        if (courseStudent == null)
            return new NotFoundViewResult(
                nameof(CourseStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        try
        {
            await _courseStudentsRepository.DeleteAsync(courseStudent);

            await _courseStudentsRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
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
                        $"The {nameof(CourseStudent)} with the ID " + //******* SERIA STUDENT OU COURSE???
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

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (InvalidOperationException ex)
        {
            // Handle the exception
            Console.WriteLine("An InvalidOperationException occurred: " +
                              ex.Message);

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
                ItemGuid = Guid.Empty,
                ItemName = courseStudent.Student.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }

    }


    private bool SchoolClassStudentExists(int id)
    {
        return _courseStudentsRepository.ExistAsync(id).Result;
    }


    private void FillViewLists(
        int courseId = 0, int studentId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        ViewData[nameof(CourseDiscipline.CourseId)] =
            new SelectList(_courseRepository.GetCourses(),
                nameof(Course.Id),
                // nameof(Course.Name),
                $"{nameof(Course.Code)} ({nameof(Course.Acronym)})",
                courseId);


        var test = _studentRepository.GetAll().ToList();
        ViewData[nameof(CourseStudent.StudentId)] =
            new SelectList(test,
                nameof(Student.Id),
                // $"{nameof(Student.FirstName)} {nameof(Student.LastName)}",
                nameof(Student.FullName),
                studentId);


        ViewData[nameof(CourseDiscipline.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                createdById);

        ViewData[nameof(CourseDiscipline.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                updatedById);
    }
}