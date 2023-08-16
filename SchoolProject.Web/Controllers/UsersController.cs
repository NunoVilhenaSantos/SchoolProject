using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
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

    // GET: Users
    public async Task<IActionResult> Index()
    {
        // return _context.Users != null
        //     ? View(await _context.Users.ToListAsync())
        //     : Problem("Entity set 'DataContextMySql.Users'  is null.");

        if (_context.Users == null)
            return Problem("Entity set 'DataContextMySql.Users' is null.");

        // var usersWithRoles = await _context.Users
        //     .Join(_context.UserRoles,
        //         user => user.Id,
        //         userRole => userRole.UserId,
        //         (user, userRole) => new
        //         {
        //             User = user, UserRole = userRole
        //         })
        //     .Join(_context.Roles, userUserRole =>
        //             userUserRole.UserRole.RoleId,
        //         role => role.Id,
        //         (userUserRole, role) => new
        //         {
        //             userUserRole.User, Role = role
        //         })
        //     .ToListAsync();
        // return View(
        //     usersWithRoles.AsEnumerable().Select(userWithRole =>
        //     {
        //         var user = userWithRole.User;
        //         var role = userWithRole.Role;
        //         user.Id = role.Name;
        //         return user;
        //     }).ToList());

        var usersWithRoles = await _context.Users
            .Join(_context.UserRoles, user => user.Id,
                userRole => userRole.UserId,
                (user, userRole) => new
                {
                    User = user, UserRole = userRole
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

        return View(usersWithRoles);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null || _context.Users == null) return NotFound();

        var user = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (user == null) return NotFound();

        return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "FirstName,LastName,Address,WasDeleted,ProfilePhotoId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")]
        User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(user);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Users == null) return NotFound();

        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    // POST: Users/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        [Bind(
            "FirstName,LastName,Address,WasDeleted,ProfilePhotoId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")]
        User user)
    {
        if (id != user.Id) return NotFound();

        if (ModelState.IsValid)
        {
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

        return View(user);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.Users == null) return NotFound();

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
        if (_context.Users == null)
            return Problem("Entity set 'DataContextMySql.Users'  is null.");

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