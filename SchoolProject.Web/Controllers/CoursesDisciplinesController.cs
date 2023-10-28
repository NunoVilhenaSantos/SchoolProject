using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Data.Repositories.Disciplines;
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
///     Courses and Disciplines Controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesDisciplinesController : Controller
{
    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(CourseDiscipline.CourseId);
    internal const string CurrentClass = nameof(CourseDiscipline);
    internal const string CurrentAction = nameof(Index);

    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CoursesDisciplinesController));


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

    private readonly UserManager<AppUser> _userManager;


    //  repositories
    private readonly ICourseDisciplinesRepository _courseDisciplinesRepository;
    private readonly IDisciplineRepository _disciplineRepository;
    private readonly ICourseRepository _courseRepository;

    // data context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     School class with courses
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="userManager"></param>
    /// <param name="courseDisciplinesRepository"></param>
    /// <param name="disciplineRepository"></param>
    /// <param name="courseRepository"></param>
    public CoursesDisciplinesController(
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        ICourseDisciplinesRepository courseDisciplinesRepository,
        IDisciplineRepository disciplineRepository,
        ICourseRepository courseRepository)
    {
        _courseDisciplinesRepository = courseDisciplinesRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
        _disciplineRepository = disciplineRepository;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _courseRepository = courseRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userManager = userManager;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
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
    ///     CourseDisciplineNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CourseDisciplineNotFound()
    {
        return View();
    }


    private List<CourseDisciplineViewModel> GetCoursesDisciplinesList()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var coursesDisciplines =
            _courseDisciplinesRepository.GetCourseDisciplines()
                .Include(e => e.CreatedBy)
                .Include(e => e.UpdatedBy)
                .ToList();

        var viewModelList = coursesDisciplines.Select(item =>
            new CourseDisciplineViewModel
            {
                Id = item.Id,
                IdGuid = item.IdGuid,
                CourseCode = item.Course.Code,
                CourseAcronym = item.Course.Acronym,
                CourseName = item.Course.Name,
                DisciplineCode = item.Discipline.Code,
                DisciplineName = item.Discipline.Name,
                DisciplineDescription = item.Discipline.Description,
                WasDeleted = item.WasDeleted,
                CreatedAt = item.CreatedAt,
                CreatedByFullName = item.CreatedBy.FullName,
                UpdatedAt = item.UpdatedAt,
                UpdatedByFullName = item.UpdatedBy?.FullName,
            }).ToList();


        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Tempo de execução: " +
                          "GetCoursesDisciplinesList " +
                          $"{elapsedMilliseconds} ms");


        return viewModelList;
    }


    // private List<CourseDiscipline> SessionData<T>() where T : class
    // {
    //     // Obtém todos os registos
    //     List<CourseDiscipline> recordsQuery;
    //
    //     // Tente obter a lista de professores da sessão
    //     if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
    //     {
    //         // Se a lista estiver na sessão, desserializa-a
    //         var json = Encoding.UTF8.GetString(allData);
    //
    //         recordsQuery =
    //             JsonConvert.DeserializeObject<List<CourseDiscipline>>(json) ??
    //             new List<CourseDiscipline>();
    //     }
    //     else
    //     {
    //         // Caso contrário, obtenha a lista completa do banco de dados
    //         // Chame a função GetTeachersList com o tipo T
    //         recordsQuery = GetCoursesDisciplinesList();
    //
    //         PaginationViewModel<T>.Initialize(_hostingEnvironment);
    //
    //         var json = PaginationViewModel<CourseDiscipline>
    //             .StoreListToFileInJson(recordsQuery);
    //
    //         // Armazene a lista na sessão para uso futuro
    //         HttpContext.Session.Set(SessionVarName,
    //             Encoding.UTF8.GetBytes(json));
    //     }
    //
    //     return recordsQuery;
    // }


    private List<CourseDisciplineViewModel> SessionDataNew<T>() where T : class
    {
        // Obtém todos os registos
        List<CourseDisciplineViewModel> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert
                    .DeserializeObject<List<CourseDisciplineViewModel>>(json) ??
                new List<CourseDisciplineViewModel>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetCoursesDisciplinesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<CourseDisciplineViewModel>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: courseDiscipline
    /// <summary>
    ///     Index, list of school class with courses
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

        // var recordsQuery = SessionData<CourseDiscipline>();
        var recordsQuery = SessionDataNew<CourseDisciplineViewModel>();

        return View(recordsQuery);
    }


    // GET: courseDiscipline
    /// <summary>
    ///     Index with cards, list of school class with courses
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

        // var recordsQuery = SessionData<CourseDiscipline>();
        var recordsQuery = SessionDataNew<CourseDisciplineViewModel>();

        return View(recordsQuery);
    }


    // GET: courseDiscipline
    /// <summary>
    ///     Index with cards, list of school class with courses
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

        // var recordsQuery = SessionData<CourseDiscipline>();
        var recordsQuery = SessionDataNew<CourseDisciplineViewModel>();

        // var model = new PaginationViewModel<CourseDiscipline>(
        //     recordsQuery,
        //     pageNumber, pageSize,
        //     recordsQuery.Count,
        //     sortOrder, sortProperty
        // );

        var model = new PaginationViewModel<CourseDisciplineViewModel>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: courseDiscipline/Details/5
    /// <summary>
    ///     Details, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(CourseDisciplineNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        // var courseDiscipline = await _courseDisciplinesRepository
        //     .GetCourseDisciplineById(id.Value).FirstOrDefaultAsync();
        var courseDiscipline = await _courseDisciplinesRepository
            .GetCourseDisciplineByGuid(idGuid.Value).FirstOrDefaultAsync();

        return courseDiscipline == null
            ? new NotFoundViewResult(nameof(CourseDisciplineNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(courseDiscipline);
    }


    // GET: courseDiscipline/Create
    /// <summary>
    ///     Create, school class with courses
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        // ViewData["DisciplineId"] =
        //     new SelectList(_context.Disciplines,
        //         "Id", "Code");

        // ViewData["CreatedById"] =
        //     new SelectList(_context.Users,
        //         "Id", "Id");

        // ViewData["DisciplineId"] =
        //     new SelectList(_context.Courses,
        //         "Id", "Acronym");

        // ViewData["UpdatedById"] =
        //     new SelectList(_context.Users,
        //         "Id", "Id");

        var courseDiscipline = new CourseDiscipline
        {
            CourseId = 0, Course = null, DisciplineId = 0, Discipline = null,
            CreatedBy = null, CreatedById = null, CreatedAt = DateTime.Now,
        };

        FillViewLists();

        return View(courseDiscipline);
    }


    // POST: courseDiscipline/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, school class with courses
    /// </summary>
    /// <param name="courseDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CourseDiscipline courseDiscipline)
    {
        if (ModelState.IsValid)
        {
            await _courseDisciplinesRepository.CreateAsync(courseDiscipline);

            HttpContext.Session.Remove(SessionVarName);

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }

        FillViewLists(
            courseDiscipline.CourseId, courseDiscipline.DisciplineId,
            courseDiscipline.CreatedBy.Id, courseDiscipline.UpdatedBy.Id);

        return View(courseDiscipline);
    }


    // GET: courseDiscipline/Edit/5
    /// <summary>
    ///     Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(CourseDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // var courseDiscipline = await _courseDisciplinesRepository
        //     .GetCourseDisciplineById(id.Value).FirstOrDefaultAsync();
        var courseDiscipline = await _courseDisciplinesRepository
            .GetCourseDisciplineByGuid(idGuid.Value).FirstOrDefaultAsync();

        if (courseDiscipline == null)
            return new NotFoundViewResult(
                nameof(CourseDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        FillViewLists(
            courseDiscipline.CourseId, courseDiscipline.DisciplineId,
            courseDiscipline.CreatedBy.Id, courseDiscipline.UpdatedBy.Id);

        return View(courseDiscipline);
    }

    // POST: courseDiscipline/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <param name="courseDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, Guid idGuid, CourseDiscipline courseDiscipline)
    {
        if (id != courseDiscipline.CourseId ||
            idGuid != courseDiscipline.IdGuid)
            return new NotFoundViewResult(
                nameof(CourseDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                await _courseDisciplinesRepository
                    .UpdateAsync(courseDiscipline);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_courseDisciplinesRepository.ExistAsync(idGuid).Result)
                    return new NotFoundViewResult(
                        nameof(CourseDisciplineNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));

                throw;
            }

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }


        FillViewLists(
            courseDiscipline.CourseId, courseDiscipline.DisciplineId,
            courseDiscipline.CreatedBy.Id, courseDiscipline.UpdatedBy.Id);

        return View(courseDiscipline);
    }


    // GET: courseDiscipline/Delete/5
    /// <summary>
    ///     Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(CourseDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // var courseDiscipline = await _courseDisciplinesRepository
        //     .GetCourseDisciplineById(id.Value).FirstOrDefaultAsync();
        var courseDiscipline = await _courseDisciplinesRepository
            .GetCourseDisciplineByGuid(idGuid.Value).FirstOrDefaultAsync();

        return courseDiscipline == null
            ? new NotFoundViewResult(nameof(CourseDisciplineNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(courseDiscipline);
    }


    // POST: courseDiscipline/Delete/5
    /// <summary>
    ///     Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, Guid idGuid)
    {
        // var courseDiscipline = await _courseDisciplinesRepository
        //     .GetCourseDisciplineById(id.Value).FirstOrDefaultAsync();
        var courseDiscipline = await _courseDisciplinesRepository
            .GetCourseDisciplineByGuid(idGuid).FirstOrDefaultAsync();

        if (courseDiscipline == null)
            return new NotFoundViewResult(
                nameof(CourseDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _courseDisciplinesRepository.DeleteAsync(courseDiscipline);

            await _courseDisciplinesRepository.SaveAllAsync();

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
                        $"The {nameof(Course)} with the ID " +
                        $"{courseDiscipline.Id} - {courseDiscipline.Course.Name} " +
                        $"{courseDiscipline.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Course),
                    ItemId = courseDiscipline.Id.ToString(),
                    ItemGuid = courseDiscipline.IdGuid,
                    ItemName = courseDiscipline.Course.Name,
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
                ItemClass = nameof(Course),
                ItemId = courseDiscipline.Id.ToString(),
                ItemGuid = courseDiscipline.IdGuid,
                ItemName = courseDiscipline.Course.Name
            };


            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        /******* INSERIDO DEVIDO A COMPLEXIDADE DA CLASSE ************/
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
                    $"The {nameof(CourseDiscipline)} with the ID " +
                    $"{courseDiscipline.Id} - {courseDiscipline.Course.Name} " +
                    $"{courseDiscipline.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(CourseDiscipline),
                ItemId = courseDiscipline.Id.ToString(),
                ItemGuid = courseDiscipline.IdGuid,
                ItemName = courseDiscipline.Course.Name
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
                    $"The {nameof(CourseDiscipline)} with the ID " +
                    $"{courseDiscipline.Id} - {courseDiscipline.Course.Name} " +
                    $"{courseDiscipline.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(CourseDiscipline),
                ItemId = courseDiscipline.Id.ToString(),
                ItemGuid = courseDiscipline.IdGuid,
                ItemName = courseDiscipline.Course.Name
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private async Task<bool> SchoolClassCourseExists(int id)
    {
        return await _courseDisciplinesRepository.ExistAsync(id);
    }


    private void FillViewLists(
        int courseId = 0, int disciplineId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        var test = _disciplineRepository.GetDisciplines().ToList();

        ViewData[nameof(CourseDiscipline.DisciplineId)] =
            new SelectList(test,
                nameof(Discipline.Id),
                $"{nameof(Discipline.Code)}",
                disciplineId);

        ViewData[nameof(CourseDiscipline.CourseId)] =
            new SelectList(_courseRepository.GetCourses(),
                nameof(Course.Id),
                nameof(Course.Name),
                courseId);

        ViewData[nameof(CourseDiscipline.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                createdById);

        ViewData[nameof(CourseDiscipline.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                updatedById);
    }
}