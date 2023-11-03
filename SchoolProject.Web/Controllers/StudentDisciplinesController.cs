using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Disciplines;
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
///     StudentDisciplinesController
/// </summary>
// [Authorize(Roles = "Admin,SuperUser,Functionary")]
[Authorize(Roles = "SuperUser,Functionary")]
public class StudentDisciplinesController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(StudentDiscipline.Id);
    internal const string CurrentClass = nameof(StudentDiscipline);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IDisciplineRepository _disciplineRepository;


    // Host Environment
    // Obtém o ambiente de hospedagem
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;


    //  repositories
    // Obtém o repositório de studentDiscipline
    private readonly IStudentDisciplineRepository _studentDisciplineRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUserHelper _userHelper;
    private readonly UserManager<AppUser> _userManager;

    // Obtém o contexto do banco de dados
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     StudentDisciplinesController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentDisciplineRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="disciplineRepository"></param>
    /// <param name="studentRepository"></param>
    /// <param name="userManager"></param>
    public StudentDisciplinesController(
        DataContextMySql context,
        IStudentDisciplineRepository studentDisciplineRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor,
        IDisciplineRepository disciplineRepository,
        IStudentRepository studentRepository, UserManager<AppUser> userManager)
    {
        // _context = context;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _httpContextAccessor = httpContextAccessor;
        _disciplineRepository = disciplineRepository;
        _studentRepository = studentRepository;
        _userManager = userManager;
        _studentDisciplineRepository = studentDisciplineRepository;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(StudentDisciplinesController));


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
    ///     StudentDisciplineNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult StudentDisciplineNotFound()
    {
        return View();
    }


    private List<StudentDiscipline> StudentAndDisciplinesList()
    {
        // return _context.StudentDisciplines
        //     .Include(s => s.Student)
        //     .Include(s => s.Discipline)
        //     .Include(s => s.CreatedBy)
        //     .Include(s => s.UpdatedBy)
        //     .AsNoTracking()
        //     .ToList();

        return _studentDisciplineRepository.GetAll()
            .Include(s => s.Student)
            .Include(s => s.Discipline)
            // .Include(s => s.CreatedBy)
            // .Include(s => s.UpdatedBy)
            .AsNoTracking()
            .ToList();
    }


    private List<StudentDiscipline> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<StudentDiscipline> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<StudentDiscipline>>(json) ??
                new List<StudentDiscipline>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = StudentAndDisciplinesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<StudentDiscipline>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: StudentDisciplines
    /// <summary>
    ///     Index, list all StudentDisciplines
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

        var recordsQuery = SessionData<StudentDiscipline>();

        return View(recordsQuery);
    }


    // GET: StudentDisciplines
    /// <summary>
    ///     Index with cards, list all StudentDisciplines
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

        var recordsQuery = SessionData<StudentDiscipline>();

        return View(recordsQuery);
    }


    // GET: StudentDisciplines
    /// <summary>
    ///     Index with cards, list all StudentDisciplines
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

        var recordsQuery = SessionData<StudentDiscipline>();

        var model = new PaginationViewModel<StudentDiscipline>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: StudentDisciplines/Details/5
    /// <summary>
    ///     Details, show details of a studentDiscipline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var studentDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _studentDisciplineRepository
                .GetStudentDisciplineById(id.Value)
                .FirstOrDefaultAsync()
            : await _studentDisciplineRepository
                .GetStudentDisciplineByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.Student)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        return studentDiscipline == null
            ? new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(studentDiscipline);
    }


    // GET: StudentDisciplines/Create
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        FillViewDatas();

        var temp = new StudentDiscipline
        {
            Student = null, Discipline = null,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result,
            CreatedAt = DateTime.Now
        };

        return View(temp);
    }


    // POST: StudentDisciplines/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, create a new studentDiscipline
    /// </summary>
    /// <param name="studentDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentDiscipline studentDiscipline)
    {
        var courseDiscipline1 = new StudentDiscipline
        {
            Discipline = await _disciplineRepository
                .GetByIdAsync(studentDiscipline.DisciplineId)
                .FirstOrDefaultAsync(),
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            Student = await _studentRepository
                .GetByIdAsync(studentDiscipline.StudentId).FirstOrDefaultAsync()
        };

        try
        {
            await _studentDisciplineRepository.CreateAsync(studentDiscipline);
            HttpContext.Session.Remove(SessionVarName);
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }


        FillViewDatas(
            studentDiscipline.StudentId,
            studentDiscipline.DisciplineId,
            studentDiscipline.CreatedById,
            studentDiscipline.UpdatedById);

        return View(studentDiscipline);
    }


    // GET: StudentDisciplines/Edit/5
    /// <summary>
    ///     Edit, edit a studentDiscipline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var studentDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _studentDisciplineRepository
                .GetStudentDisciplineById(id.Value)
                .FirstOrDefaultAsync()
            : await _studentDisciplineRepository
                .GetStudentDisciplineByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.Student)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        if (studentDiscipline == null)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        FillViewDatas(
            studentDiscipline.StudentId,
            studentDiscipline.DisciplineId,
            studentDiscipline.CreatedById,
            studentDiscipline.UpdatedById);

        return View(studentDiscipline);
    }

    // POST: StudentDisciplines/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, edit a studentDiscipline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <param name="studentDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, Guid idGuid, StudentDiscipline studentDiscipline)
    {
        if (id != studentDiscipline.Id ||
            idGuid != studentDiscipline.IdGuid)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        if (ModelState.IsValid)
        {
            try
            {
                await _studentDisciplineRepository.UpdateAsync(
                    studentDiscipline);

                await _studentDisciplineRepository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_studentDisciplineRepository.ExistAsync(id).Result)
                    return new NotFoundViewResult(
                        nameof(StudentDisciplineNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));

                throw;
            }

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }


        FillViewDatas(
            studentDiscipline.StudentId,
            studentDiscipline.DisciplineId,
            studentDiscipline.CreatedById,
            studentDiscipline.UpdatedById);

        return View(studentDiscipline);
    }


    // GET: StudentDisciplines/Delete/5
    /// <summary>
    ///     Delete, delete a studentDiscipline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var studentDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _studentDisciplineRepository
                .GetStudentDisciplineById(id.Value)
                .FirstOrDefaultAsync()
            : await _studentDisciplineRepository
                .GetStudentDisciplineByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.Student)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        return studentDiscipline == null
            ? new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(studentDiscipline);
    }


    // POST: StudentDisciplines/Delete/5
    /// <summary>
    ///     Delete, delete a studentDiscipline
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, Guid idGuid)
    {
        var studentDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _studentDisciplineRepository
                .GetStudentDisciplineById(id)
                .FirstOrDefaultAsync()
            : await _studentDisciplineRepository
                .GetStudentDisciplineByIdGuid(idGuid)
                // .Include(s => s.Discipline)
                // .Include(s => s.Student)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        if (studentDiscipline == null)
            return new NotFoundViewResult(
                nameof(StudentDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        try
        {
            await _studentDisciplineRepository.DeleteAsync(studentDiscipline);

            await _studentDisciplineRepository.SaveAllAsync();

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
                        $"The {nameof(StudentDiscipline)} with the ID " +
                        $"{studentDiscipline.Id} - " +
                        $"{studentDiscipline.Student.FullName} " +
                        $"{studentDiscipline.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(StudentDiscipline),
                    ItemId = studentDiscipline.Id.ToString(),
                    ItemGuid = studentDiscipline.IdGuid,
                    ItemName = studentDiscipline.Student.FullName
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
                ItemClass = nameof(StudentDiscipline),
                ItemId = studentDiscipline.Id.ToString(),
                ItemGuid = studentDiscipline.IdGuid,
                ItemName = studentDiscipline.Student.FullName
            };

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
                    $"The {nameof(StudentDiscipline)} with the ID " +
                    $"{studentDiscipline.Id} - {studentDiscipline.Student.FullName} " +
                    $"{studentDiscipline.IdGuid} " +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(StudentDiscipline),
                ItemId = studentDiscipline.Id.ToString(),
                ItemGuid = studentDiscipline.IdGuid,
                ItemName = studentDiscipline.Student.FullName
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
                    $"The {nameof(StudentDiscipline)} with the ID " +
                    $"{studentDiscipline.Id} - {studentDiscipline.Student.FullName} " +
                    $"{studentDiscipline.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(StudentDiscipline),
                ItemId = studentDiscipline.Id.ToString(),
                ItemGuid = studentDiscipline.IdGuid,
                ItemName = studentDiscipline.Student.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private void FillViewDatas(
        int studentId = 0, int disciplineId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        ViewData[nameof(StudentDiscipline.DisciplineId)] =
            new SelectList(
                _disciplineRepository.GetDisciplines().Select(d =>
                    new {d.Id, Description = $"{d.Code} - {d.Name}"}).ToList(),
                nameof(Discipline.Id),
                "Description",
                disciplineId);

        ViewData[nameof(StudentDiscipline.StudentId)] =
            new SelectList(_studentRepository.GetAll().ToList(),
                nameof(Student.Id),
                nameof(Student.FullName),
                studentId);


        ViewData[nameof(StudentDiscipline.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                createdById);

        ViewData[nameof(StudentDiscipline.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                updatedById);
    }
}