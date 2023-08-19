using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin,SuperUser")]
public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;


    /// <summary>
    ///     Constructor for the RolesController
    /// </summary>
    /// <param name="roleManager"></param>
    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }


    // GET: Roles
    /// <summary>
    ///     Action to show all the roles
    /// </summary>
    /// <returns>a list of roles</returns>
    [HttpGet]
    public Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles;

        return Task.FromResult<IActionResult>(View(roles));

        // return View(roles);
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
        if (id == null) return NotFound();

        var roles = await _roleManager.FindByIdAsync(id);

        if (roles == null) return NotFound();

        return View(roles);
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

        var result = await _roleManager.CreateAsync(new IdentityRole
        {
            Name = identityRole.Name
        });

        if (result.Succeeded)
            return RedirectToAction("Index");

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
        if (id == null) return NotFound();

        var role = await _roleManager.FindByIdAsync(id);

        if (role == null) return NotFound();

        return View(role);
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
        if (id != identityRole.Id) return NotFound();

        if (!ModelState.IsValid) return View(identityRole);

        try
        {
            await _roleManager.CreateAsync(identityRole);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _roleManager.FindByIdAsync(id) == null)
                return NotFound();
            throw;
        }

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
        if (id == null) return NotFound();

        var role = await _roleManager.FindByIdAsync(id);

        if (role == null) return NotFound();

        return View(role);
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

        if (identityRole != null) await _roleManager.DeleteAsync(identityRole);


        await _roleManager.Roles.ExecuteDeleteAsync();

        return RedirectToAction(nameof(Index));
    }
}