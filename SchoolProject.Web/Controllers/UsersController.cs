using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin,SuperUser")]
public class UsersController : Controller
{
    private readonly DataContextMySql _context;


    public UsersController(DataContextMySql context)
    {
        _context = context;
    }




    private IEnumerable<UserWithRolesViewModel> GetUsersWithRolesList()
    {
        var usersWithRoles =  _context.Users
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
                       Roles = new List<string> { role.Name }
                   })
           .ToListAsync();


        return usersWithRoles.Result ?? Enumerable.Empty<UserWithRolesViewModel>();
    }






    // GET: Users
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }


    // GET: Users
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }


    // GET: Users
    [HttpGet]
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }


    // GET: Users
    [HttpGet]
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetUsersWithRolesList());
    }




    // GET: Users
    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    var usersWithRoles = await _context.Users
    //        .Join(_context.UserRoles, user => user.Id,
    //            userRole => userRole.UserId,
    //            (user, userRole) => new
    //            {
    //                User = user, UserRole = userRole
    //            })
    //        .Join(_context.Roles,
    //            userUserRole =>
    //                userUserRole.UserRole.RoleId,
    //            role => role.Id,
    //            (userUserRole, role) =>
    //                new UserWithRolesViewModel
    //                {
    //                    User = userUserRole.User,
    //                    // You can modify this if users can have multiple roles
    //                    Role = userUserRole.UserRole,
    //                    Roles = new List<string> {role.Name}
    //                })
    //        .ToListAsync();

    //    return View(usersWithRoles);
    //}


    // GET: Users/Details/5
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