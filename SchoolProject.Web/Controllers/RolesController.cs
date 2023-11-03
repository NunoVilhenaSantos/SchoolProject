using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     RolesController class, inherits from Controller.
///     Authorization is required to access this controller.
///     Roles that can access this controller are Admin and SuperUser.
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class RolesController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(IdentityRole.Name);
    internal const string CurrentClass = nameof(IdentityRole);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMailHelper _mailHelper;


    //  repositories
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly UserManager<AppUser> _userManager;


    /// <summary>
    ///     Constructor for the RolesController
    /// </summary>
    /// <param name="roleManager"></param>
    /// <param name="userManager"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="converterHelper"></param>
    public RolesController(AuthenticatedUserInApp authenticatedUserInApp,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment hostingEnvironment,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager)
    {
        _authenticatedUserInApp = authenticatedUserInApp;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _roleManager = roleManager;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _userManager = userManager;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(IdentityRole));


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


    private List<IdentityRole> GetRolesList()
    {
        return _roleManager.Roles.AsNoTracking().ToList();
    }


    private List<IdentityRole> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<IdentityRole> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            return JsonConvert.DeserializeObject<List<IdentityRole>>(json) ??
                   new List<IdentityRole>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = GetRolesList();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 = PaginationViewModel<IdentityRole>
            .StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

        return recordsQuery;
    }


    // GET: Roles
    /// <summary>
    ///     Action to show all the roles
    /// </summary>
    /// <returns>a list of roles</returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<IdentityRole>();
        return View(recordsQuery);
    }


    // GET: Roles
    /// <summary>
    ///     Action to show all the roles
    /// </summary>
    /// <returns>a list of roles</returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<IdentityRole>();

        return View(recordsQuery);
    }


    // GET: Roles
    /// <summary>
    ///     Action to show all the roles
    /// </summary>
    /// <returns>a list of roles</returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<IdentityRole>();

        var model = new PaginationViewModel<IdentityRole>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Roles/Details/5
    /// <summary>
    ///     Action to show the details of a role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index));

        var roles = await _roleManager.FindByIdAsync(id);

        return roles == null
            ? new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index))
            : View(roles);
    }


    // GET: Roles/Create
    /// <summary>
    ///     Action to call the view to create a new role
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Roles/Create
    //
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Action to validate the model a create a new role
    /// </summary>
    /// <param name="identityRole"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IdentityRole identityRole)
    {
        if (!ModelState.IsValid) return View(identityRole);

        var roleExists =
            identityRole.Name != null &&
            await _roleManager.RoleExistsAsync(identityRole.Name);

        if (roleExists)
        {
            ModelState.AddModelError(
                "Roles", "This role already exists");

            return View(identityRole);
        }

        var result = await _roleManager.CreateAsync(
            new IdentityRole
            {
                Name = identityRole.Name
            });

        if (result.Succeeded)
        {
            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("Roles", error.Description);

        return View(identityRole);
    }


    // GET: Roles/Edit/5
    /// <summary>
    ///     Action to call the view to edit a role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index));

        var role = await _roleManager.FindByIdAsync(id);

        return role == null
            ? new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index))
            : View(role);
    }


    // POST: Roles/Edit/5
    //
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    //
    /// <summary>
    ///     Action to validate and then edit a role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="identityRole"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, IdentityRole identityRole)
    {
        if (id != identityRole.Id)
            return new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index));

        if (!ModelState.IsValid) return View(identityRole);

        try
        {
            await _roleManager.CreateAsync(identityRole);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _roleManager.FindByIdAsync(id) == null)
                return new NotFoundViewResult(
                    nameof(RoleNotFound), CurrentClass, id, CurrentController,
                    nameof(Index));
            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Roles/Delete/5
    /// <summary>
    ///     Action to call the view to delete a role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index));

        var role = await _roleManager.FindByIdAsync(id);

        return role == null
            ? new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index))
            : View(role);
    }


    // POST: Roles/Delete/5
    /// <summary>
    ///     Action to validate the deletion of the role
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var identityRole =
            await _roleManager.FindByIdAsync(id);

        if (identityRole == null)
            return new NotFoundViewResult(
                nameof(RoleNotFound), CurrentClass, id, CurrentController,
                nameof(Index));


        var appUserList =
            await _userManager.GetUsersInRoleAsync(identityRole.Name);

        if (appUserList.Count > 0)
        {
            ModelState.AddModelError("Roles",
                "Este papel não pode ser excluído, " +
                "pois está associado a um ou mais AppUsers.\n" +
                "Portanto, o sistema não permitirá a exclusão.\n\n");

            ModelState.AddModelError("Roles",
                "This role cannot be deleted because it is associated with one or more AppUsers.\n" +
                "Therefore, the system will not allow its deletion.");

            return View(identityRole);
        }

        await _roleManager.DeleteAsync(identityRole);

        // Remove all roles from the system
        // await _roleManager.Roles.ExecuteDeleteAsync();


        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     RoleNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult RoleNotFound()
    {
        return View();
    }

    // -------------------------------------------------------------- //

    /// <summary>
    ///     GetRolesListJson action.
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Roles/GetRolesListJson")]
    [Route("Roles/GetRolesListJson")]
    public Task<JsonResult> GetRolesListJson()
    {
        var rolesList = _roleManager.Roles
            .Select(p => new SelectListItem
                {Text = p.Name, Value = p.Id.ToString()})
            .OrderBy(c => c.Text).ToList();

        rolesList.Insert(0, new SelectListItem
            {Text = "(Select a Subscription....)", Value = "0"});


        return Task.FromResult(Json(rolesList));
    }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}