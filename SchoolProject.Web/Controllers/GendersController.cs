using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Repositories.Genders;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     GendersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class GendersController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Gender.Name);
    internal const string CurrentClass = nameof(Gender);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;


    //  repositories
    private readonly IGenderRepository _genderRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;

    // data-contexts
    // private readonly DataContext _context;
    // private readonly DataContextMySql _contextMySql;


    /// <summary>
    ///     GendersController constructor.
    /// </summary>
    /// <param name="genderRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="storageHelper"></param>
    public GendersController(
        IGenderRepository genderRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor)
    {
        // _context = context;
        // _contextMySql = contextMySql;
        _genderRepository = genderRepository;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _httpContextAccessor = httpContextAccessor;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(GendersController));


    // Obtém o controlador atual
    internal string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }


    private List<Gender> GendersList()
    {
        return _genderRepository.GetAll().AsNoTracking().ToList();
    }


    private List<Gender> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Gender> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            return JsonConvert.DeserializeObject<List<Gender>>(json) ??
                   new List<Gender>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = GendersList();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 =
            PaginationViewModel<Gender>.StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

        return recordsQuery;
    }


    // GET: Genders
    /// <summary>
    ///     Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Gender>();

        return View(recordsQuery);
    }


    // GET: Genders
    /// <summary>
    ///     Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Gender>();

        return View(recordsQuery);
    }


    // GET: Genders
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Gender>();

        var model = new PaginationViewModel<Gender>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Genders/Details/5
    /// <summary>
    ///     Details action, to open the view for details.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var gender = await _genderRepository.GetByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        if (gender == null)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        return View(gender);
    }


    // GET: Genders/Create
    /// <summary>
    ///     Create action, to open the view for creating.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }


    // POST: Genders/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create action validation and confirmation.
    /// </summary>
    /// <param name="gender"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Gender gender)
    {
        if (!ModelState.IsValid) return View(gender);

        await _genderRepository.CreateAsync(gender);

        await _genderRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }

    // GET: Genders/Edit/5
    /// <summary>
    ///     Edit action, to open the view for editing.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var gender = await _genderRepository.GetByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        return gender == null
            ? new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(gender);
    }


    // POST: Genders/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit action validation and confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gender"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Gender gender)
    {
        if (id != gender.Id)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(gender);

        try
        {
            await _genderRepository.UpdateAsync(gender);
            await _genderRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _genderRepository.ExistAsync(gender.Id))
                return new NotFoundViewResult(
                    nameof(GenderNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Genders/Delete/5
    /// <summary>
    ///     Delete action, to open the view for confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var gender = await _genderRepository.GetByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        return gender == null
            ? new NotFoundViewResult(nameof(GenderNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(gender);
    }


    // POST: Genders/Delete/5
    /// <summary>
    ///     Delete action confirmed.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gender =
            await _genderRepository.GetByIdAsync(id).FirstOrDefaultAsync();

        if (gender == null)
            return new NotFoundViewResult(
                nameof(GenderNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _genderRepository.DeleteAsync(gender);

            await _genderRepository.SaveAllAsync();

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
                        $"The {nameof(Gender)} with the ID " +
                        $"{gender.Id} - {gender.Name} - {gender.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Gender),
                    ItemId = gender.Id.ToString(),
                    ItemGuid = gender.IdGuid,
                    ItemName = gender.Name
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
                ItemClass = nameof(Gender),
                ItemId = gender.Id.ToString(),
                ItemGuid = gender.IdGuid,
                ItemName = gender.Name
            };


            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    /// <summary>
    ///     Gender Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult GenderNotFound()
    {
        return View();
    }


    // -------------------------------------------------------------- //


    // -------------------------------------------------------------- //

    /// <summary>
    ///     GetGendersListJson action.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Genders/GetGendersListJson")]
    [Route("Genders/GetGendersListJson")]
    public Task<JsonResult> GetGendersListJson()
    {
        var gendersList =
            _genderRepository.GetComboGenders();

        return Task.FromResult(Json(gendersList.OrderBy(c => c.Text)));
    }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}