using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Disciplines;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     disciplines controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class DisciplinesController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Discipline.Name);
    internal const string CurrentClass = nameof(Discipline);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;


    //  repositories
    private readonly IDisciplineRepository _disciplineRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly UserManager<AppUser> _userManager;

    // data context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     constructor for the courses controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="courseRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="converterHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="userManager"></param>
    public DisciplinesController(
        DataContextMySql context,
        IDisciplineRepository courseRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IHttpContextAccessor httpContextAccessor,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        UserManager<AppUser> userManager)
    {
        _authenticatedUserInApp = authenticatedUserInApp;
        _httpContextAccessor = httpContextAccessor;
        _disciplineRepository = courseRepository;
        _hostingEnvironment = hostingEnvironment;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _userManager = userManager;
        // _context = context;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(DisciplinesController));


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


    private List<Discipline> GetDisciplinesList()
    {
        return _disciplineRepository.GetDisciplines().AsNoTracking().ToList();
    }


    private List<Discipline> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Discipline> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<Discipline>>(json) ??
                new List<Discipline>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetDisciplinesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Discipline>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Disciplines
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Discipline>();

        return View(recordsQuery);
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Disciplines
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Discipline>();

        return View(recordsQuery);
    }


    // GET: Disciplines
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Discipline>();

        var model = new PaginationViewModel<Discipline>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Disciplines/Details/5
    /// <summary>
    ///     Details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var discipline = await _disciplineRepository.GetDisciplineById(id.Value)
            .FirstOrDefaultAsync();

        return discipline == null
            ? new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(discipline);
    }


    // GET: Disciplines/Create
    /// <summary>
    ///     Create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        var discipline = new Discipline
        {
            Code = null,
            Name = null,
            Description = null,
            Hours = 25,
            CreditPoints = 2.5,
            ProfilePhotoId = default,
            CreatedAt = DateTime.Now,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result,
        };

        return View(discipline);
    }


    // POST: Disciplines/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create action
    /// </summary>
    /// <param name="discipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Discipline discipline)
    {
        //if (!ModelState.IsValid) return View(discipline);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var imageId = discipline.ProfilePhotoId;

        if (discipline.ImageFile is {Length: > 0})
            imageId =
                await _storageHelper.UploadStorageAsync(
                    discipline.ImageFile, BucketName);

        discipline.ProfilePhotoId = imageId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var discipline1 = new Discipline
        {
            Code = discipline.Code,
            Name = discipline.Name,
            Description = discipline.Description,
            Hours = discipline.Hours,
            CreditPoints = discipline.CreditPoints,
            ProfilePhotoId = discipline.ProfilePhotoId,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result
        };


        await _disciplineRepository.CreateAsync(discipline1);

        await _disciplineRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Disciplines/Edit/5
    /// <summary>
    ///     Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var discipline = await _disciplineRepository
            .GetDisciplineById(id.Value)
            .FirstOrDefaultAsync();

        return discipline == null
            ? new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(discipline);
    }


    // POST: Disciplines/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="discipline"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Discipline discipline)
    {
        if (id != discipline.Id)
            return new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        // if (!ModelState.IsValid) return View(student);

        var discipline1 = await _disciplineRepository
            .GetDisciplineById(id).FirstOrDefaultAsync();

        if (discipline1 == null) return View(discipline);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = discipline.ProfilePhotoId;

        if (discipline.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    discipline.ImageFile, BucketName);

        discipline.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        // discipline1.Code = discipline.Code;
        discipline1.Name = discipline.Name;
        discipline1.Description = discipline.Description;
        discipline1.Hours = discipline.Hours;
        discipline1.CreditPoints = discipline.CreditPoints;
        discipline1.ProfilePhotoId = discipline.ProfilePhotoId;
        // discipline1.CreatedBy = discipline.CreatedBy;
        // discipline1.CreatedById =discipline.CreatedById;
        discipline1.UpdatedBy =
            _authenticatedUserInApp.GetAuthenticatedUser().Result;


        try
        {
            await _disciplineRepository.UpdateAsync(discipline1);

            await _disciplineRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _disciplineRepository.ExistAsync(discipline.Id))
                return new NotFoundViewResult(
                    nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }
    }


    // GET: Countries/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var discipline = await _disciplineRepository
            .GetDisciplineById(id.Value)
            .FirstOrDefaultAsync();


        return discipline == null
            ? new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(discipline);
    }


    // POST: Discipline/Delete/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var discipline = await _disciplineRepository.GetByIdAsync(id)
            .FirstOrDefaultAsync();

        if (discipline == null)
            return new NotFoundViewResult(
                nameof(DisciplineNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _disciplineRepository.DeleteAsync(discipline);

            await _disciplineRepository.SaveAllAsync();

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
                        $"The {nameof(Discipline)} with the ID " +
                        $"{discipline.Id} - {discipline.Name} " +
                        $"{discipline.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Discipline),
                    ItemId = discipline.Id.ToString(),
                    ItemGuid = discipline.IdGuid,
                    ItemName = discipline.Name
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
                ItemClass = nameof(Discipline),
                ItemId = discipline.Id.ToString(),
                ItemGuid = discipline.IdGuid,
                ItemName = discipline.Name
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }

    private bool CourseExists(int id)
    {
        return _disciplineRepository.ExistAsync(id).Result;
    }


    /// <summary>
    ///     Discipline Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult DisciplineNotFound()
    {
        return View();
    }


    private void FillViewLists(
        int courseId = 0, int disciplineId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        var test = _disciplineRepository.GetDisciplines().ToList();

        // ViewData[nameof(Discipline.DisciplineId)] =
        //     new SelectList(test,
        //         nameof(Discipline.Id),
        //         $"{nameof(Discipline.Code)}",
        //         disciplineId);

        // ViewData[nameof(Discipline.CourseId)] =
        //     new SelectList(_courseRepository.GetCourses(),
        //         nameof(discipline.Id),
        //         nameof(discipline.Name),
        //         courseId);

        ViewData[nameof(Discipline.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                createdById);

        ViewData[nameof(Discipline.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                updatedById);
    }
}