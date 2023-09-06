using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     RolesController class, inherits from Controller.
///     Authorisation is required to access this controller.
///     Roles that can access this controller are Admin and SuperUser.
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class RolesController : Controller
{
    private const string SessionVarName = "AllRolesList";
    private const string BucketName = "roles";
    private const string SortProperty = "Name";

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IWebHostEnvironment _hostingEnvironment;


    /// <summary>
    ///     Constructor for the RolesController
    /// </summary>
    /// <param name="roleManager"></param>
    /// <param name="hostingEnvironment"></param>
    public RolesController(RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment hostingEnvironment)
    {
        _roleManager = roleManager;
        _hostingEnvironment = hostingEnvironment;
    }


    private List<IdentityRole> GetRolesList()
    {
        return _roleManager.Roles.ToList();
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

            recordsQuery =
                JsonConvert.DeserializeObject<List<IdentityRole>>(json) ??
                new List<IdentityRole>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetRolesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<IdentityRole>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

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
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

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

        var result = await _roleManager.CreateAsync(new()
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