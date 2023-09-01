using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///   UsersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class UsersController : Controller
{
    private readonly DataContextMySql _context;
    private readonly IUserHelper _userHelper;


    /// <summary>
    ///  UsersController constructor.
    /// </summary>
    /// <param name="userHelper"></param>
    /// <param name="context"></param>
    public UsersController(
        IUserHelper userHelper,
        DataContextMySql context
    )
    {
        _context = context;
        _userHelper = userHelper;
    }


    // GET: Users
    /// <summary>
    ///  Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }


    private IEnumerable<UserWithRolesViewModel> GetUsersWithRolesList()
    {
        var usersWithRoles = _context.Users
            .Join(_context.UserRoles, user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new
                {
                    User = user,
                    UserRole = userRole
                })
            .Join(_context.Roles,
                userUserRole =>
                    userUserRole.UserRole.RoleId,
                role => role.Id,
                (userUserRole, role) =>
                    new UserWithRolesViewModel
                    {
                        User = userUserRole.User,
                        // You can modify this if users can have multiple roles
                        Role = userUserRole.UserRole,
                        Roles = new List<string> {role.Name}
                    })
            .ToListAsync();


        return usersWithRoles.Result;
    }


    // GET: Users
    /// <summary>
    /// IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }


    // GET: Roles
    // /// <summary>
    // ///     Action to show all the roles
    // /// </summary>
    // /// <returns>a list of roles</returns>
    // [HttpGet]
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records =
    //         GetUsersWithRoles(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<UserWithRolesViewModel>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Users.Count()
    //     };
    //
    //     return View(model);
    // }


    private List<UserWithRolesViewModel> GetUsersWithRoles(
        int pageNumber, int pageSize)
    {
        var usersWithRoles = GetUsersWithRolesList();

        return usersWithRoles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


    // GET: Roles
    /// <summary>
    ///     Action to show all the roles
    /// </summary>
    /// <returns>a list of roles</returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records =
            GetUsersWithRoles(pageNumber, pageSize);

        var model = new PaginationViewModel<UserWithRolesViewModel>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Users.Count()
        };

        return View(model);
    }


    // GET: Users/Details/5
    /// <summary>
    ///  Details method, to open the details view of specific id.
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
    /// Create method, to open the create view.
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
    /// Create method, to create a new user.
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
    /// Edit method, to open the edit view of specific id.
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
    /// Edit method, to edit a specific user.
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
    /// Delete method, to open the delete view of specific id.
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
    /// Delete method, to delete a specific user, confirmation.
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