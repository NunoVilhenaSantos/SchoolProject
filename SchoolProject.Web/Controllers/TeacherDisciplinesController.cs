using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Disciplines;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Teachers and Disciplines Controller class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class TeacherDisciplinesController : Controller
{
    // Obtém o tipo da classe atual
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(TeacherDiscipline.Id);
    internal const string CurrentClass = nameof(TeacherDiscipline);
    internal const string CurrentAction = nameof(Index);
    internal static string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IDisciplineRepository _disciplineRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;


    //  repositories
    private readonly ITeacherDisciplineRepository _teacherDisciplineRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUserHelper _userHelper;

    // datacontext
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     TeacherDisciplinesController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherDisciplineRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="storageHelper"></param>
    /// <param name="disciplineRepository"></param>
    /// <param name="teacherRepository"></param>
    public TeacherDisciplinesController(
        DataContextMySql context,
        ITeacherDisciplineRepository teacherDisciplineRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor,
        IDisciplineRepository disciplineRepository,
        ITeacherRepository teacherRepository)
    {
        // _context = context;
        _teacherDisciplineRepository = teacherDisciplineRepository;
        _authenticatedUserInApp = authenticatedUserInApp;
        _disciplineRepository = disciplineRepository;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _teacherRepository = teacherRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(TeacherDisciplinesController));


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
    ///     Teacher Discipline Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TeacherDisciplineNotFound()
    {
        return View();
    }


    private List<TeacherDiscipline> TeacherDisciplinesList()
    {
        var teacherCoursesList = _teacherDisciplineRepository
            .GetTeacherDisciplines()
            .AsNoTracking().ToList();

        return teacherCoursesList;
    }


    private List<TeacherDiscipline> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<TeacherDiscipline> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<TeacherDiscipline>>(json) ??
                new List<TeacherDiscipline>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = TeacherDisciplinesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<TeacherDiscipline>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: TeacherDisciplines
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<TeacherDiscipline>();

        return View(recordsQuery);
    }


    // GET: TeacherDisciplines
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<TeacherDiscipline>();

        return View(recordsQuery);
    }


    // GET: TeacherDisciplines
    /// <summary>
    ///     IndexCards1 method for the cards view with pagination, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<TeacherDiscipline>();

        var model = new PaginationViewModel<TeacherDiscipline>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: TeacherDisciplines/Details/5
    /// <summary>
    ///     Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var teacherDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _teacherDisciplineRepository
                .GetTeacherDisciplinesById(id.Value)
                .FirstOrDefaultAsync()
            : await _teacherDisciplineRepository
                .GetTeacherDisciplinesByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        return teacherDiscipline == null
            ? new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacherDiscipline);
    }


    // GET: TeacherDisciplines/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        FillViewDatas();

        return View();
    }

    // POST: TeacherDisciplines/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <param name="teacherDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeacherDiscipline teacherDiscipline)
    {
        if (ModelState.IsValid)
        {
            await _teacherDisciplineRepository.CreateAsync(teacherDiscipline);

            await _teacherDisciplineRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }

        FillViewDatas(
            teacherDiscipline.TeacherId, teacherDiscipline.DisciplineId,
            teacherDiscipline.CreatedById, teacherDiscipline.UpdatedById);

        return View(teacherDiscipline);
    }


    // GET: TeacherDisciplines/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var teacherDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _teacherDisciplineRepository
                .GetTeacherDisciplinesById(id.Value)
                .FirstOrDefaultAsync()
            : await _teacherDisciplineRepository
                .GetTeacherDisciplinesByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        if (teacherDiscipline == null)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        FillViewDatas(
            teacherDiscipline.TeacherId, teacherDiscipline.DisciplineId,
            teacherDiscipline.CreatedById, teacherDiscipline.UpdatedById);

        return View(teacherDiscipline);
    }


    // POST: TeacherDisciplines/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <param name="teacherDiscipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, Guid idGuid, TeacherDiscipline teacherDiscipline)
    {
        if (id != teacherDiscipline.Id ||
            idGuid != teacherDiscipline.IdGuid)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        if (ModelState.IsValid)
        {
            try
            {
                await _teacherDisciplineRepository.UpdateAsync(
                    teacherDiscipline);

                HttpContext.Session.Remove(SessionVarName);

                await _teacherDisciplineRepository.SaveAllAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(teacherDiscipline.TeacherId))
                    return new NotFoundViewResult(
                        nameof(TeacherDisciplineNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));

                throw;
            }

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }


        FillViewDatas(
            teacherDiscipline.TeacherId, teacherDiscipline.DisciplineId,
            teacherDiscipline.CreatedById, teacherDiscipline.UpdatedById);

        return View(teacherDiscipline);
    }


    // GET: TeacherDisciplines/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null || idGuid == null || idGuid == Guid.Empty)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var teacherDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _teacherDisciplineRepository
                .GetTeacherDisciplinesById(id.Value)
                .FirstOrDefaultAsync()
            : await _teacherDisciplineRepository
                .GetTeacherDisciplinesByIdGuid(idGuid.Value)
                // .Include(s => s.Discipline)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        return teacherDiscipline == null
            ? new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacherDiscipline);
    }


    // POST: TeacherDisciplines/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, Guid idGuid)
    {
        var teacherDiscipline = id != 0 || idGuid == Guid.Empty
            ? await _teacherDisciplineRepository
                .GetTeacherDisciplinesById(id)
                .FirstOrDefaultAsync()
            : await _teacherDisciplineRepository
                .GetTeacherDisciplinesByIdGuid(idGuid)
                // .Include(s => s.Discipline)
                // .Include(s => s.CreatedBy)
                // .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync();


        if (teacherDiscipline == null)
            return new NotFoundViewResult(
                nameof(TeacherDisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        try
        {
            await _teacherDisciplineRepository.DeleteAsync(teacherDiscipline);

            await _teacherDisciplineRepository.SaveAllAsync();

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
                        $"The {nameof(TeacherDiscipline)} with the ID " +
                        $"{teacherDiscipline.Id} - {teacherDiscipline.Teacher.FullName} " +
                        $"{teacherDiscipline.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(TeacherDiscipline),
                    ItemId = teacherDiscipline.Id.ToString(),
                    ItemGuid = teacherDiscipline.IdGuid,
                    ItemName = teacherDiscipline.Teacher.FullName
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
                ItemClass = nameof(TeacherDiscipline),
                ItemId = teacherDiscipline.Id.ToString(),
                ItemGuid = teacherDiscipline.IdGuid,
                ItemName = teacherDiscipline.Teacher.FullName
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
                    $"The {nameof(TeacherDiscipline)} with the ID " +
                    $"{teacherDiscipline.Id} - {teacherDiscipline.Discipline.Name} " +
                    $"{teacherDiscipline.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(TeacherDiscipline),
                ItemId = teacherDiscipline.Id.ToString(),
                ItemGuid = teacherDiscipline.IdGuid,
                ItemName = teacherDiscipline.Discipline.Name
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
                    $"The {nameof(TeacherDiscipline)} with the ID " +
                    $"{teacherDiscipline.Id} - {teacherDiscipline.Discipline.Name} " +
                    $"{teacherDiscipline.IdGuid} " +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(TeacherDiscipline),
                ItemId = teacherDiscipline.Id.ToString(),
                ItemGuid = teacherDiscipline.IdGuid,
                ItemName = teacherDiscipline.Discipline.Name
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private bool TeacherCourseExists(int id)
    {
        return _teacherRepository.ExistAsync(id).Result;
    }


    private void FillViewDatas(
        int teacherId = 0, int disciplineId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        ViewData[nameof(TeacherDiscipline.DisciplineId)] =
            new SelectList(
                _disciplineRepository.GetDisciplines().Select(d =>
                    new {d.Id, Description = $"{d.Code} - {d.Name}"}).ToList(),
                nameof(Discipline.Id),
                "Description",
                disciplineId);

        ViewData[nameof(TeacherDiscipline.TeacherId)] =
            new SelectList(_teacherRepository.GetTeachers(),
                nameof(Teacher.Id),
                nameof(Teacher.FullName),
                teacherId);

        ViewData[nameof(TeacherDiscipline.CreatedById)] =
            new SelectList(_userHelper.GetUsersWithFullName(),
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                createdById);

        ViewData[nameof(TeacherDiscipline.UpdatedById)] =
            new SelectList(_userHelper.GetUsersWithFullName(),
                nameof(AppUser.Id),
                nameof(AppUser.FullName),
                updatedById);
    }
}