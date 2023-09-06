using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     UsersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class UsersController : Controller
{
    internal const string SessionVarName = "AllUsersWithRolesList";
    private const string BucketName = "users";
    private const string SortProperty = "FirstName";

    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly DataContextMySql _context;
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
        IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _userHelper = userHelper;
        _hostingEnvironment = hostingEnvironment;
    }


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
                        User = user.User,
                        UserRole = userRole
                    })
            .GroupJoin(_context.Roles,
                userUserRole => userUserRole.UserRole.RoleId,
                role => role.Id,
                (userUserRole,
                        roles) =>
                    new UserWithRolesViewModel
                    {
                        User = userUserRole.User,
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
            return NotFound();

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (user == null) return NotFound();

        return View(user);
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
    ///     Create method, to create a new user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user)
    {
        if (!ModelState.IsValid) return View(user);

        _context.Add(user);

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
            return NotFound();

        var user = await _context.Users.FindAsync(id);

        if (user == null) return NotFound();

        return View(user);
    }

    // POST: Users/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, to edit a specific user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, User user)
    {
        if (id != user.Id) return NotFound();

        if (!ModelState.IsValid) return View(user);

        try
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(user.Id))
                return NotFound();
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
            return NotFound();

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (user == null) return NotFound();

        return View(user);
    }

    // POST: Users/Delete/5
    /// <summary>
    ///     Delete method, to delete a specific user, confirmation.
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