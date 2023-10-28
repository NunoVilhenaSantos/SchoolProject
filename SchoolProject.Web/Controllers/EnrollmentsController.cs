using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Disciplines;
using SchoolProject.Web.Data.Repositories.Enrollments;
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
///     EnrollmentsController class.
/// </summary>
// [Authorize(Roles = "Admin,SuperUser,Functionary")]
[Authorize(Roles = "SuperUser,Functionary,Student")]
public class EnrollmentsController : Controller
{
    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Enrollment.DisciplineId);
    internal const string CurrentClass = nameof(Enrollment);
    internal const string CurrentAction = nameof(Index);

    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(EnrollmentsController));


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
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IDisciplineRepository _disciplineRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly UserManager<AppUser> _userManager;

    // data context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     EnrollmentsController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="enrollmentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="disciplineRepository"></param>
    /// <param name="studentRepository"></param>
    public EnrollmentsController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IEnrollmentRepository enrollmentRepository,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        IDisciplineRepository disciplineRepository,
        IStudentRepository studentRepository)
    {
        // _context = context;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _userManager = userManager;
        _disciplineRepository = disciplineRepository;
        _studentRepository = studentRepository;
        _storageHelper = storageHelper;
        _converterHelper = converterHelper;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _enrollmentRepository = enrollmentRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
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
    ///     Enrollment Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult EnrollmentNotFound()
    {
        return View();
    }


    private List<Enrollment> GetEnrollmentsWithCoursesAndStudents()
    {
        // var enrollmentsWithStudent =
        //     _context.Enrollments
        //         .Include(e => e.Discipline)
        //         .Include(e => e.Student)
        //         // .Include(e => e.CreatedBy)
        //         // .Include(e => e.UpdatedBy)
        //         .ToList();

        return _enrollmentRepository.GetAll()
            .Include(e => e.Discipline)
            .Include(e => e.Student)
            // .Include(e => e.CreatedBy)
            // .Include(e => e.UpdatedBy)
            .ToList();
    }


    private List<Enrollment> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Enrollment> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<Enrollment>>(json) ??
                new List<Enrollment>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetEnrollmentsWithCoursesAndStudents();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Enrollment>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Enrollments
    /// <summary>
    ///     Index method, for the main view.
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

        var recordsQuery = SessionData<Enrollment>();

        return View(recordsQuery);
    }


    // GET: Enrollments
    /// <summary>
    ///     IndexCards method for the cards view.
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

        var recordsQuery = SessionData<Enrollment>();

        return View(recordsQuery);
    }


    // GET: Enrollments
    /// <summary>
    ///     IndexCards method for the cards view.
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

        var recordsQuery = SessionData<Enrollment>();

        var model = new PaginationViewModel<Enrollment>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Enrollments/Details/5
    /// <summary>
    ///     Details method.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _enrollmentRepository.GetEnrollmentById(id.Value)
            // .Include(e => e.Discipline)
            // .Include(e => e.Student)
            // .Include(e => e.CreatedBy)
            // .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(enrollment);
    }


    // GET: Enrollments/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        FillViewLists();

        return View();
    }

    // POST: Enrollments/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for adding a new enrollment.
    /// </summary>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            await _enrollmentRepository.CreateAsync(enrollment);

            await _enrollmentRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }

        FillViewLists(
            enrollment.DisciplineId, enrollment.StudentId,
            enrollment.CreatedBy.Id, enrollment.UpdatedBy?.Id);

        return View(enrollment);
    }


    // GET: Enrollments/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _enrollmentRepository.GetEnrollmentById(id.Value)
            // .Include(e => e.Discipline)
            // .Include(e => e.Student)
            // .Include(e => e.CreatedBy)
            // .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (enrollment == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        FillViewLists(
            enrollment.DisciplineId, enrollment.StudentId,
            enrollment.CreatedBy.Id, enrollment.UpdatedBy?.Id);

        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for editing a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Enrollment enrollment)
    {
        if (id != enrollment.StudentId)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (!ModelState.IsValid)
        {
            FillViewLists(
                enrollment.DisciplineId, enrollment.StudentId,
                enrollment.CreatedBy.Id, enrollment.UpdatedBy?.Id);

            return View(enrollment);
        }

        try
        {
            await _enrollmentRepository.UpdateAsync(enrollment);

            await _enrollmentRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnrollmentExists(enrollment.StudentId))
                return new NotFoundViewResult(nameof(EnrollmentNotFound),
                    CurrentClass, id.ToString(), CurrentController,
                    nameof(Index));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }

    // GET: Enrollments/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _enrollmentRepository.GetEnrollmentById(id.Value)
            // .Include(e => e.Discipline)
            // .Include(e => e.Student)
            // .Include(e => e.CreatedBy)
            // .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(enrollment);
    }


    // POST: Enrollments/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for deleting a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(id)
            .FirstOrDefaultAsync();

        if (enrollment == null)
            return new NotFoundViewResult(
                nameof(EnrollmentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _enrollmentRepository.DeleteAsync(enrollment);

            await _enrollmentRepository.SaveAllAsync();

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
                        $"The {nameof(Enrollment)} with the ID " +
                        $"{enrollment.Id} - {enrollment.Student.FullName} " +
                        $"{enrollment.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Enrollment),
                    ItemId = enrollment.Id.ToString(),
                    ItemGuid = enrollment.IdGuid,
                    ItemName = enrollment.Student.FullName
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
                ItemClass = nameof(Enrollment),
                ItemId = enrollment.Id.ToString(),
                ItemGuid = enrollment.IdGuid,
                ItemName = enrollment.Student.FullName
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
                    $"The {nameof(Enrollment)} with the ID " +
                    $"{enrollment.Id} - {enrollment.Student.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(Enrollment),
                ItemId = enrollment.Id.ToString(),
                ItemGuid = enrollment.IdGuid,
                ItemName = enrollment.Student.FullName
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
                    $"The {nameof(Enrollment)} with the ID " +
                    $"{enrollment.Id} - {enrollment.Student.FullName} " +
                    $"{enrollment.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(Enrollment),
                ItemId = enrollment.Id.ToString(),
                ItemGuid = enrollment.IdGuid,
                ItemName = enrollment.Student.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private bool EnrollmentExists(int id)
    {
        return _enrollmentRepository.ExistAsync(id).Result;
    }


    private void FillViewLists(
        int disciplineId = 0, int studentId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        var test1 = _disciplineRepository.GetDisciplines().AsEnumerable();
        ViewData[nameof(Enrollment.DisciplineId)] =
            new SelectList(test1,
                nameof(Discipline.Id),
                $"{nameof(Discipline.Code)}",
                disciplineId);


        var test2 = _studentRepository.GetAll().AsEnumerable();
        ViewData[nameof(Enrollment.StudentId)] =
            new SelectList(_studentRepository.GetAll().AsEnumerable(),
                nameof(Student.Id),
                nameof(Student.FullName),
                studentId);

        ViewData[nameof(Enrollment.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                createdById);

        ViewData[nameof(Enrollment.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                updatedById);
    }
}