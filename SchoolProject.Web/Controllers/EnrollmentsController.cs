using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
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
using SchoolProject.Web.Models.Enrollments;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     EnrollmentsController class.
/// </summary>
// [Authorize(Roles = "Admin,SuperUser,Functionary")]
[Authorize(Roles = "SuperUser,Functionary,Student")]
public class EnrollmentsController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Enrollment.DisciplineId);
    internal const string CurrentClass = nameof(Enrollment);

    internal const string CurrentAction = nameof(IndexCards1);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IDisciplineRepository _disciplineRepository;


    //  repositories
    private readonly IEnrollmentRepository _enrollmentRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStorageHelper _storageHelper;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserHelper _userHelper;
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
    /// <param name="roleManager"></param>
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
        IStudentRepository studentRepository,
        RoleManager<IdentityRole> roleManager)
    {
        // _context = context;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _userManager = userManager;
        _disciplineRepository = disciplineRepository;
        _studentRepository = studentRepository;
        _roleManager = roleManager;
        _storageHelper = storageHelper;
        _converterHelper = converterHelper;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _enrollmentRepository = enrollmentRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
    }

    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(EnrollmentsController));


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
            .AsNoTracking()
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


    private async Task<List<Enrollment>> GetEnrollmentsForViews()
    {
        var user = await _userHelper.GetUserByEmailAsync(User.Identity?.Name);
        var userRoles = await _userManager.GetRolesAsync(user);

        List<Enrollment> recordsQuery;

        if (userRoles.Contains("Admin") || userRoles.Contains("SuperUser") || userRoles.Contains("Functionary"))
            recordsQuery = SessionData<Enrollment>();

        else if (userRoles.Contains("Student"))
            recordsQuery = _enrollmentRepository
                .GetEnrollmentByEmail(User.Identity.Name)
                .Where(e => e.Student.Email == User.Identity.Name).ToList();

        else
            recordsQuery = new List<Enrollment>();


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
    public async Task<IActionResult> Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // var recordsQuery = SessionData<Enrollment>();
        var recordsQuery = await GetEnrollmentsForViews();

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
    public async Task<IActionResult> IndexCards(int pageNumber = 1,
        int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // var recordsQuery = SessionData<Enrollment>();
        var recordsQuery = await GetEnrollmentsForViews();

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
    public async Task<IActionResult> IndexCards1(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)

    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // var recordsQuery = SessionData<Enrollment>();
        var recordsQuery = await GetEnrollmentsForViews();

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
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(EnrollmentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        var enrollment = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id.Value)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(IndexCards1))
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

            return RedirectToAction(nameof(IndexCards1));
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
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));


        var enrollment = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id.Value)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        if (enrollment == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));

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
    /// <param name="idGuid"></param>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, Guid idGuid, Enrollment enrollment)
    {
        if (id != enrollment.Id || idGuid != enrollment.IdGuid)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));



        var enrollment1 = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid)
                .FirstOrDefaultAsync();


        if (enrollment1 == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));

        enrollment1.Grade = enrollment.Grade;
        enrollment1.Absences = enrollment.Absences;

        //if (!ModelState.IsValid)
        //{
        //    FillViewLists(
        //        enrollment.DisciplineId, enrollment.StudentId,
        //        enrollment.CreatedById, enrollment.UpdatedById);

        //    return View(enrollment);
        //}


        try
        {
            await _enrollmentRepository.UpdateAsync(enrollment1);

            await _enrollmentRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnrollmentExists(enrollment.Id))
                return new NotFoundViewResult(nameof(EnrollmentNotFound),
                    CurrentClass, id.ToString(), CurrentController,
                    nameof(IndexCards1));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(IndexCards1));
    }


    // GET: Enrollments/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));


        var enrollment = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id.Value)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(IndexCards1))
            : View(enrollment);
    }


    // POST: Enrollments/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for deleting a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, Guid idGuid)
    {
        var enrollment = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid)
                .FirstOrDefaultAsync();


        if (enrollment == null)
            return new NotFoundViewResult(
                nameof(EnrollmentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1));


        try
        {
            await _enrollmentRepository.DeleteAsync(enrollment);

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


    // GET: GradesAssignment/Edit/5
    /// <summary>
    ///     GradesAssignment method, for the GradesAssignment view.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="studentIdGuid"></param>
    /// <returns></returns>
    //public async Task<IActionResult> GradesAssignment(
    //    int? studentId, Guid? studentIdGuid)
    //{
    //    if (studentId == null || studentIdGuid == null ||
    //        studentIdGuid == Guid.Empty)
    //        return new NotFoundViewResult(nameof(EnrollmentNotFound),
    //            StudentsController.CurrentClass, studentIdGuid.ToString(),
    //            HomeController.SplitCamelCase(nameof(StudentsController)),
    //            nameof(Index));

    //    List<Enrollment> enrollments =
    //        studentId != 0 || studentIdGuid == Guid.Empty
    //            ? await _enrollmentRepository
    //                .GetEnrollments()
    //                .Where(e => e.StudentId == studentId)
    //                .ToListAsync()
    //            : await _enrollmentRepository
    //                .GetEnrollments()
    //                .Where(e => e.StudentIdGuid == studentIdGuid)
    //                .ToListAsync();

    //    if (enrollments == null)
    //        return new NotFoundViewResult(nameof(EnrollmentNotFound),
    //            CurrentClass, studentId.ToString(), CurrentController,
    //            nameof(IndexCards1));

    //    ViewData["studentId"] = studentId;
    //    ViewData["studentIdGuid"] = studentIdGuid;

    //    return View(enrollments);
    //}

    // POST: Enrollments/GradesAssignment/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    // /// <summary>
    // ///     Edit method, for editing a enrollment.
    // /// </summary>
    // /// <param name="studentId"></param>
    // /// <param name="studentIdGuid"></param>
    // /// <param name="enrollments"></param>
    // /// <returns></returns>
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> GradesAssignment(
    //     int studentId, Guid studentIdGuid, List<Enrollment> enrollments)
    // {
    //     if (studentId != enrollments[0].StudentId ||
    //         studentIdGuid != enrollments[0].StudentIdGuid ||
    //         studentIdGuid == Guid.Empty)
    //         return new NotFoundViewResult(nameof(EnrollmentNotFound),
    //             StudentsController.CurrentClass, studentIdGuid.ToString(),
    //             HomeController.SplitCamelCase(nameof(StudentsController)),
    //             nameof(Index));
    //
    //
    //     if (!ModelState.IsValid)
    //     {
    //         enrollments = studentId != 0 || studentIdGuid == Guid.Empty
    //             ? await _enrollmentRepository
    //                 .GetEnrollments()
    //                 .Where(e => e.StudentId == studentId)
    //                 .ToListAsync()
    //             : await _enrollmentRepository
    //                 .GetEnrollments()
    //                 .Where(e => e.StudentIdGuid == studentIdGuid)
    //                 .ToListAsync();
    //
    //
    //         if (enrollments == null)
    //             return new NotFoundViewResult(nameof(EnrollmentNotFound),
    //                 CurrentClass, studentId.ToString(), CurrentController,
    //                 nameof(IndexCards1));
    //
    //         ViewData["studentId"] = studentId;
    //         ViewData["studentIdGuid"] = studentIdGuid;
    //
    //
    //         return View(enrollments);
    //     }
    //
    //
    //     try
    //     {
    //         // await _enrollmentRepository.UpdateAsync(enrollment);
    //
    //         await _enrollmentRepository.SaveAllAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         // if (!EnrollmentExists(enrollment.StudentId))
    //         //     return new NotFoundViewResult(nameof(EnrollmentNotFound),
    //         //         CurrentClass, studentId.ToString(), CurrentController,
    //         //         nameof(IndexCards1));
    //
    //         Console.WriteLine(
    //             "something went wrong! " +
    //             "Controler: EnrollmentsController.cs, " +
    //             "Action: GradesAssignment");
    //
    //         throw;
    //     }
    //
    //     HttpContext.Session.Remove(SessionVarName);
    //
    //     return RedirectToAction(nameof(IndexCards1));
    // }
    // POST: Enrollments/GradesAssignment/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    ///// <summary>
    /////     Edit method, for editing a enrollment.
    ///// </summary>
    ///// <param name="studentId"></param>
    ///// <param name="studentIdGuid"></param>
    ///// <param name="enrollments"></param>
    ///// <returns></returns>
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> GradesAssignmentOld(int studentId, Guid studentIdGuid, List<EnrollmentViewModel> enrollments)
    //{
    //    if (enrollments.Any(e => e.EnrollmentId != 0 || e.EnrollmentIdGuid != Guid.Empty))
    //        return BadRequest("Os valores de Id e IdGuid não correspondem às matrículas fornecidas.");

    //    if (!ModelState.IsValid)
    //    {
    //        // Se o modelo não for válido, retorne a vista com o modelo atualizado
    //        ViewData["studentId"] = studentId;
    //        ViewData["studentIdGuid"] = studentIdGuid;
    //        return View(enrollments);
    //    }

    //    try
    //    {
    //        // Atualize as matrículas correspondentes com as informações em enrollments
    //        foreach (var enrollmentViewModel in enrollments)
    //        {
    //            var enrollment = await _enrollmentRepository.GetByIdGuidAsync(enrollmentViewModel.EnrollmentIdGuid).FirstOrDefaultAsync();
    //            if (enrollment != null)
    //            {
    //                // Atualize apenas as propriedades relevantes
    //                enrollment.Grade = enrollmentViewModel.Grade;
    //                enrollment.Absences = enrollmentViewModel.Absences;
    //            }
    //        }

    //        await _enrollmentRepository.SaveAllAsync();

    //        // Redirecione para a ação desejada após a atualização
    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch (Exception ex)
    //    {
    //        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar as notas.");
    //        ViewData["studentId"] = studentId;
    //        ViewData["studentIdGuid"] = studentIdGuid;
    //        return View(enrollments);
    //    }
    //}

    // POST: Enrollments/GradesAssignment/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    ///// <summary>
    /////     Edit method, for editing a enrollment.
    ///// </summary>
    ///// <param name="studentId"></param>
    ///// <param name="studentIdGuid"></param>
    ///// <param name="enrollments"></param>
    ///// <returns></returns>
    //[HttpPost]
    //// [ValidateAntiForgeryToken]
    //public async Task<IActionResult> GradesAssignment(int studentId, Guid studentIdGuid, List<EnrollmentViewModel> enrollments)
    //{
    //    if (enrollments.Any(e => e.EnrollmentId == 0 && (e.EnrollmentIdGuid == null || e.EnrollmentIdGuid == Guid.Empty)))
    //        return BadRequest("Os valores de Id e IdGuid não correspondem às matrículas fornecidas.");

    //    Console.WriteLine("studentId: " + studentId);

    //    // Verifique se a solicitação contém dados.
    //    if (!ModelState.IsValid || HttpContext.Request.Method != "POST" || !HttpContext.Request.HasFormContentType)
    //    {
    //        // A solicitação não é um POST ou não contém dados de formulário.
    //        return StatusCode(503, "A solicitação é inválida.");
    //    }

    //    // Obtenha os dados do corpo da solicitação.
    //    var form = HttpContext.Request.Form;

    //    // Obtenha o corpo da solicitação como uma string JSON
    //    var requestBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

    //    // Agora você pode desserializar o JSON em uma lista de objetos EnrollmentViewModel, se necessário.
    //    var enrollments1 = JsonConvert.DeserializeObject<List<EnrollmentViewModel>>(requestBody);

    //    // Verifique se os dados são válidos.
    //    if (enrollments1 == null || enrollments1.Count == 0)
    //    {
    //        ModelState.AddModelError(string.Empty, "Os dados da solicitação estão inválidos.");

    //        // Passe o modelo para a vista
    //        ViewData["StudentId"] = studentId;
    //        ViewData["StudentIdGuid"] = studentIdGuid;
    //        ViewData["StudentFullName"] =
    //            _enrollmentRepository
    //            .GetEnrollments()
    //            .Where(e => e.StudentId == studentId)
    //            .FirstOrDefault()?.Student?.FullName; // Correção: Use '?.' para evitar exceções quando os objetos são nulos.

    //        return View(enrollments);
    //    }

    //    try
    //    {
    //        // Atualize as matrículas correspondentes com as informações em enrollments
    //        foreach (var enrollmentViewModel in enrollments)
    //        {
    //            var enrollment = await _enrollmentRepository.GetByIdGuidAsync(enrollmentViewModel.EnrollmentIdGuid).FirstOrDefaultAsync();
    //            if (enrollment != null)
    //            {
    //                // Atualize apenas as propriedades relevantes
    //                enrollment.Grade = enrollmentViewModel.Grade;
    //                enrollment.Absences = enrollmentViewModel.Absences;
    //            }
    //        }

    //        await _enrollmentRepository.SaveAllAsync();

    //        // Redirecione para a ação desejada após a atualização
    //        return RedirectToAction(nameof(Details), "Students", new { id = studentId });

    //        // Redirecione para a ação desejada após a atualização
    //        // return RedirectToAction(nameof(Index));
    //    }
    //    catch (Exception ex)
    //    {
    //        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar as notas.");

    //        // Passe o modelo para a vista
    //        ViewData["StudentId"] = studentId;
    //        ViewData["StudentIdGuid"] = studentIdGuid;
    //        ViewData["StudentFullName"] =
    //            _enrollmentRepository
    //            .GetEnrollments()
    //            .Where(e => e.StudentId == studentId)
    //            .FirstOrDefault().Student.FullName;

    //        // Redirecione para a ação desejada após a atualização

    //        return View(enrollments);
    //    }
    //}
    private bool EnrollmentExists(int id)
    {
        return _enrollmentRepository.ExistAsync(id).Result;
    }


    private void FillViewLists(
        int disciplineId = 0, int studentId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        ViewData[nameof(Enrollment.DisciplineId)] =
            new SelectList(
                _disciplineRepository.GetDisciplines().Select(d =>
                        new {d.Id, Description = $"{d.Code} - {d.Name}"})
                    .ToList(),
                nameof(Discipline.Id),
                "Description",
                disciplineId);

        ViewData[nameof(Enrollment.StudentId)] =
            new SelectList(_studentRepository.GetAll().ToList(),
                nameof(Student.Id),
                nameof(Student.FullName),
                studentId);

        ViewData[nameof(Enrollment.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                createdById);

        ViewData[nameof(Enrollment.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                updatedById);
    }


    // --------------------------------- --------------------------------- //
    // ------------------ Grades Assignment ------------------------------ //
    // --------------------------------- --------------------------------- //


    // GET: Enrollments/GradesAssignment/5
    /// <summary>
    ///     GradesAssignment method, for the GradesAssignment view.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="studentIdGuid"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GradesAssignment(
        int? studentId, Guid? studentIdGuid)
    {
        if (studentId == null || studentIdGuid == null ||
            studentIdGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                StudentsController.CurrentClass, studentIdGuid.ToString(),
                HomeController.SplitCamelCase(nameof(StudentsController)),
                nameof(Index));


        Console.WriteLine("studentId: " + studentId);


        var studentEnrollmentTemp = _enrollmentRepository
            .GetEnrollments()
            .Include(s => s.Student)
            .Where(e => e.StudentId == studentId)
            .AsQueryable();

        var studentFullName = _studentRepository
            .GetByIdAsync(studentId.Value).FirstOrDefault()?.FullName;

        var model = new GradesAssignmentViewModel
        {
            StudentId = studentId.Value,
            StudentIdGuid = studentIdGuid.Value,
            StudentFullName = studentFullName,
            // Obtenha as matrículas do aluno e mapeie para EnrollmentViewModel
            Enrollments = await studentEnrollmentTemp
                .Select(e => new EnrollmentViewModel
                {
                    DisciplineCode = e.Discipline.Code,
                    DisciplineName = e.Discipline.Name,

                    EnrollmentId = e.Id,
                    EnrollmentIdGuid = e.IdGuid,

                    Grade = e.Grade,
                    Absences = e.Absences,
                    PercentageOfAbsences = e.PercentageOfAbsences,
                    DisciplineWorkLoadHours = e.DisciplineWorkLoadHours
                })
                .ToListAsync()
        };


        if (model.Enrollments == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, studentId.ToString(), CurrentController,
                nameof(IndexCards1));


        // Passe o modelo para a vista
        ViewData["StudentId"] = studentId;
        ViewData["StudentIdGuid"] = studentIdGuid;
        ViewData["StudentFullName"] = studentFullName;


        return View(model);
    }


    /// <summary>
    ///     GradesAssignment method, for the GradesAssignment view.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="studentIdGuid"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    // [Route("api/Enrollments/GradesAssignment")]
    [Route(
        "Enrollments/GradesAssignment/")]
    public async Task<IActionResult> GradesAssignment(
        int studentId, Guid studentIdGuid,
        GradesAssignmentViewModel model
    )
    {
        if (model == null) return BadRequest("Dados inválidos.");


        studentId = model.StudentId;
        studentIdGuid = model.StudentIdGuid;
        var enrollments = model.Enrollments;


        // Resto do seu código para processar os dados

        if (enrollments.Any(e =>
                e.EnrollmentId == 0 && (e.EnrollmentIdGuid == null ||
                                        e.EnrollmentIdGuid == Guid.Empty)))
            return BadRequest("Os valores de Id e IdGuid não " +
                              "correspondem às matrículas fornecidas.");


        Console.WriteLine("studentId: " + studentId);


        // Assuming you have an entity for enrollments
        // Atualize as matrículas correspondentes com as informações em enrollments
        if (enrollments != null || enrollments.Count >= 0)
        {
            foreach (var enrollment in enrollments)
            {
                var enrollment1 = await _enrollmentRepository
                    .GetByIdGuidAsync(enrollment.EnrollmentIdGuid)
                    .FirstOrDefaultAsync();

                if (enrollment1 == null) continue;

                // Atualize apenas as propriedades relevantes
                enrollment.Grade = enrollment1.Grade;
                enrollment.Absences = enrollment1.Absences;
            }

            await _enrollmentRepository.SaveAllAsync();

            // Redirecione para a ação desejada após a atualização
            return RedirectToAction(nameof(Details), "Students",
                new {id = studentId});
        }


        // If the ModelState is not valid, return to the same view with validation errors.
        ModelState.AddModelError(string.Empty,
            "Ocorreu um erro ao atualizar as notas.");


        var studentEnrollmentTemp = _enrollmentRepository
            .GetEnrollments()
            .Include(s => s.Student)
            .Where(e => e.StudentId == studentId)
            .AsQueryable();

        var studentFullName = _studentRepository
            .GetByIdAsync(studentId).FirstOrDefault()?.FullName;

        model = new GradesAssignmentViewModel
        {
            StudentId = studentId,
            StudentIdGuid = studentIdGuid,
            StudentFullName = studentFullName,
            // Obtenha as matrículas do aluno e mapeie para EnrollmentViewModel
            Enrollments = await studentEnrollmentTemp
                .Select(e => new EnrollmentViewModel
                {
                    DisciplineCode = e.Discipline.Code,
                    DisciplineName = e.Discipline.Name,

                    EnrollmentId = e.Id,
                    EnrollmentIdGuid = e.IdGuid,

                    Grade = e.Grade,
                    Absences = e.Absences,
                    PercentageOfAbsences = e.PercentageOfAbsences,
                    DisciplineWorkLoadHours = e.DisciplineWorkLoadHours
                })
                .ToListAsync()
        };


        if (model.Enrollments == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, studentId.ToString(), CurrentController,
                nameof(IndexCards1));


        // Passe o modelo para a vista
        ViewData["StudentId"] = studentId;
        ViewData["StudentIdGuid"] = studentIdGuid;
        ViewData["StudentFullName"] = studentFullName;


        return View(model);
    }


    // --------------------------------- --------------------------------- //
    // ------------------ Save Enrollments ------------------------------- //
    // --------------------------------- --------------------------------- //


    /// <summary>
    ///     SaveEnrollments method, for the GradesAssignment view.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="studentIdGuid"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> SaveEnrollments(
        int? studentId, Guid? studentIdGuid)
    {
        if (studentId == null || studentIdGuid == null ||
            studentIdGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                StudentsController.CurrentClass, studentIdGuid.ToString(),
                HomeController.SplitCamelCase(nameof(StudentsController)),
                nameof(Index));


        Console.WriteLine("studentId: " + studentId);


        var studentEnrollmentTemp = _enrollmentRepository
            .GetEnrollments()
            .Include(s => s.Student)
            .Where(e => e.StudentId == studentId)
            .AsQueryable();


        // Obtenha as matrículas do aluno e mapeie para EnrollmentViewModel
        var enrollments = await studentEnrollmentTemp
            .Select(e => new EnrollmentViewModel
            {
                DisciplineCode = e.Discipline.Code,
                DisciplineName = e.Discipline.Name,

                EnrollmentId = e.Id,
                EnrollmentIdGuid = e.IdGuid,

                Grade = e.Grade,
                Absences = e.Absences,
                PercentageOfAbsences = e.PercentageOfAbsences,
                DisciplineWorkLoadHours = e.DisciplineWorkLoadHours
            })
            .ToListAsync();

        if (enrollments == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, studentId.ToString(), CurrentController,
                nameof(IndexCards1));


        // Passe o modelo para a vista
        ViewData["StudentId"] = studentId;
        ViewData["StudentIdGuid"] = studentIdGuid;
        ViewData["StudentFullName"] = _studentRepository
            .GetByIdAsync(studentId.Value).FirstOrDefault()?.FullName;


        return View(enrollments);
    }


    /// <summary>
    /// </summary>
    /// <param name="enrollmentGuid"></param>
    /// <param name="updateRequest"></param>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Enrollments/SaveEnrollments/{enrollmentGuid:guid}")]
    // [Route("Enrollments/SaveEnrollments")]
    // [HttpPost("Enrollments/SaveEnrollments/{enrollmentGuid:guid}")]
    public async Task<IActionResult> SaveEnrollments(
        Guid enrollmentGuid,
        List<EnrollmentViewModel> model,
        // EnrollmentUpdateRequest updateRequest
        // [FromBody] 
        object updateRequest
    )
    {
        var updateRequest1 =
            JsonConvert.DeserializeObject<EnrollmentUpdateRequest>(
                updateRequest.ToString());

        var enrollment = await _enrollmentRepository
            .GetByIdGuidAsync(enrollmentGuid)
            .FirstOrDefaultAsync();

        if (enrollment == null)
            return NotFound("Enrollment not found");


        // Update the relevant properties with values from the updateRequest
        enrollment.Grade = updateRequest1.Grade;
        enrollment.Absences = updateRequest1.Absences;

        await _enrollmentRepository.SaveAllAsync();

        // Return a success message indicating that the data was saved.
        return Ok("Data saved successfully.");
    }


    // [HttpPost]
    // [Route("api/Enrollments/SaveEnrollments/{enrollmentGuid:guid}")]
    // [Route("Enrollments/SaveEnrollments")]
    [HttpPost("Enrollments/SaveEnrollment/{enrollmentGuid:guid}")]
    public async Task<IActionResult> SaveEnrollment(
        Guid enrollmentGuid,
        // EnrollmentUpdateRequest updateRequest
        // [FromBody] EnrollmentUpdateRequest updateRequest
        object updateRequest
    )
    {
        var updateRequest1 =
            JsonConvert.DeserializeObject<EnrollmentUpdateRequest>(
                updateRequest.ToString());

        var enrollment = await _enrollmentRepository
            .GetByIdGuidAsync(enrollmentGuid)
            .FirstOrDefaultAsync();

        if (enrollment == null)
            return NotFound("Enrollment not found");


        // Update the relevant properties with values from the updateRequest
        enrollment.Grade = updateRequest1.Grade;
        enrollment.Absences = updateRequest1.Absences;

        await _enrollmentRepository.SaveAllAsync();

        // Return a success message indicating that the data was saved.
        return Ok("Data saved successfully.");
    }


    public class EnrollmentUpdateRequest
    {
        public decimal? Grade { get; set; }
        public int Absences { get; set; }
    }





    // --------------------------------- --------------------------------- //
    // ------------------ Student Enrollments ---------------------------- //
    // --------------------------------- --------------------------------- //







    // GET: Enrollments
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="studentIdGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> StudentEnrollments(
         int? studentId, Guid? studentIdGuid)
    {
        if (studentId == null || studentIdGuid == null ||
            studentIdGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                StudentsController.CurrentClass, studentIdGuid.ToString(),
                HomeController.SplitCamelCase(nameof(StudentsController)),
                nameof(Index));


        Console.WriteLine("studentId: " + studentId);


        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;



        var studentEnrollmentTemp = 
            await _enrollmentRepository
            .GetEnrollments()
            .Include(s => s.Student)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();



        return View(studentEnrollmentTemp);
    }





    // --------------------------------- --------------------------------- //
    // ------------------ Edit Enrollments ------------------------------- //
    // --------------------------------- --------------------------------- //





    // GET: Enrollments/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> EditEnrollment(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));


        var enrollment = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id.Value)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid.Value)
                .FirstOrDefaultAsync();


        if (enrollment == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));

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
    /// <param name="idGuid"></param>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEnrollment(
        int id, Guid idGuid, Enrollment enrollment)
    {
        if (id != enrollment.Id || idGuid != enrollment.IdGuid)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));



        var enrollment1 = id != 0 || idGuid == Guid.Empty
            ? await _enrollmentRepository
                .GetEnrollmentById(id)
                .FirstOrDefaultAsync()
            : await _enrollmentRepository
                .GetEnrollmentByIdGuid(idGuid)
                .FirstOrDefaultAsync();


        if (enrollment1 == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController,
                nameof(IndexCards1));

        enrollment1.Grade = enrollment.Grade;
        enrollment1.Absences = enrollment.Absences;

        //if (!ModelState.IsValid)
        //{
        //    FillViewLists(
        //        enrollment.DisciplineId, enrollment.StudentId,
        //        enrollment.CreatedById, enrollment.UpdatedById);

        //    return View(enrollment);
        //}


        try
        {
            await _enrollmentRepository.UpdateAsync(enrollment1);

            await _enrollmentRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnrollmentExists(enrollment.Id))
                return new NotFoundViewResult(nameof(EnrollmentNotFound),
                    CurrentClass, id.ToString(), CurrentController,
                    nameof(IndexCards1));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);
        
        // return RedirectToAction("StudentEnrollments", new { studentId = enrollment1.StudentId, studentIdGuid = enrollment1.StudentIdGuid });

        return RedirectToAction(nameof(StudentEnrollments), new { studentId = enrollment1.StudentId, studentIdGuid = enrollment1.StudentIdGuid });
    }






}