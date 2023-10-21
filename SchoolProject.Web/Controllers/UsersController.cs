using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     UsersController class.
/// </summary>
//[Authorize(Roles = "Admin,SuperUser")]
public class UsersController : Controller
{
    // Obtém o tipo da classe atual
    internal const string CurrentClass = nameof(UserWithRolesViewModel);
    internal const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string BucketName = nameof(User);

    internal const string SortProperty =
        nameof(UserWithRolesViewModel.AppUser.UserName);


    // A private field to get the data context in app.
    private readonly DataContextMySql _context;

    // A private field to get the hosting environment in app.
    private readonly IWebHostEnvironment _hostingEnvironment;


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

    // A private field to get the user helper in app.
    private readonly IUserHelper _userHelper;

    /// <summary>
    ///     UsersController constructor.
    /// </summary>
    /// <param name="userHelper"></param>
    /// <param name="context"></param>
    /// <param name="hostingEnvironment"></param>
    public UsersController(
        IUserHelper userHelper,
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp)
    {
        _context = context;
        _userHelper = userHelper;
        _hostingEnvironment = hostingEnvironment;
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
    ///     UserNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult UserNotFound => View();


    private List<UserWithRolesViewModel> GetUsersWithRolesList()
    {
        var usersWithRoles = _context.Users
            .GroupJoin(_context.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRoles) => new
                {
                    User = user,
                    UserRoles = userRoles
                })
            .SelectMany(
                x => x.UserRoles.DefaultIfEmpty(),
                (user, userRole) =>
                    new
                    {
                        user.User,
                        UserRole = userRole
                    })
            .GroupJoin(_context.Roles,
                userUserRole => userUserRole.UserRole.RoleId,
                role => role.Id,
                (userUserRole,
                        roles) =>
                    new UserWithRolesViewModel
                    {
                        AppUser = userUserRole.User,
                        Role = userUserRole.UserRole,
                        Roles = roles.Select(role => role.Name).ToList()
                    })
            .ToList();

        return usersWithRoles;
    }


    private List<UserWithRolesViewModel> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<UserWithRolesViewModel> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert
                    .DeserializeObject<List<UserWithRolesViewModel>>(json) ??
                new List<UserWithRolesViewModel>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetUsersWithRolesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json =
                PaginationViewModel<UserWithRolesViewModel>
                    .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Users
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery =
            SessionData<UserWithRolesViewModel>();
        return View(recordsQuery);
    }


    // GET: Users
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery =
            SessionData<UserWithRolesViewModel>();
        return View(recordsQuery);
    }


    // GET: Users
    /// <summary>
    ///     IndexCards1, Action to show all the roles
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery =
            SessionData<UserWithRolesViewModel>();

        var model = new PaginationViewModel<UserWithRolesViewModel>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Users/Details/5
    /// <summary>
    ///     Details method, to open the details view of specific id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        return user == null
            ? new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index))
            : View(user);
    }

    // GET: Users/Create
    /// <summary>
    ///     Create method, to open the create view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Users/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, to create a new appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppUser appUser)
    {
        if (!ModelState.IsValid) return View(appUser);

        _context.Add(appUser);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Users/Edit/5
    /// <summary>
    ///     Edit method, to open the edit view of specific id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, id,
                CurrentController, nameof(Index));

        var user = await _context.Users.FindAsync(id);

        return user == null
            ? new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index))
            : View(user);
    }

    // POST: Users/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, to edit a specific appUser.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="appUser"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, AppUser appUser)
    {
        if (id != appUser.Id)
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, id,
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(appUser);

        try
        {
            _context.Update(appUser);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(appUser.Id))
                return new NotFoundViewResult(
                    nameof(UserNotFound), CurrentClass, id,
                    CurrentController, nameof(Index));
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Users/Delete/5
    /// <summary>
    ///     Delete method, to open the delete view of specific id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, id,
                CurrentController, nameof(Index));

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        return user == null
            ? new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index))
            : View(user);
    }

    // POST: Users/Delete/5
    /// <summary>
    ///     Delete method, to delete a specific appUser, confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null) _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool UserExists(string id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}