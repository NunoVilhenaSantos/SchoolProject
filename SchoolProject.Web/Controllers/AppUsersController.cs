using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class AppUsersController : Controller
{
    // Obtém o tipo da classe atual
    internal const string SessionVarName = UsersController.SessionVarName;
    internal const string SortProperty = UsersController.SortProperty;
    internal const string CurrentClass = UsersController.CurrentClass;
    internal const string CurrentAction = nameof(Index);
    internal const string ClassRole = CurrentClass;
    internal static readonly string BucketName = UsersController.BucketName;


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // datacontexts
    private readonly DataContextMySql _context;

    // repositories


    // Helpers
    private readonly IConverterHelper _converterHelper;


    // host environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;

    private readonly IUserHelper _userHelper;
    // private readonly DataContextMySql _contextMySql;


    /// <summary>
    ///     AppUsersController constructor.
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="context"></param>
    /// <param name="authenticatedUserInApp"></param>
    public AppUsersController(
        IWebHostEnvironment hostingEnvironment,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        DataContextMySql context, AuthenticatedUserInApp authenticatedUserInApp)
    {
        _authenticatedUserInApp = authenticatedUserInApp;
        _hostingEnvironment = hostingEnvironment;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _context = context;
    }

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(AppUsersController));


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
    ///     User Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult UserNotFound()
    {
        return View();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    private List<UserWithRolesViewModel> GetUsersWithRolesList()
    {
        var usersWithRoles = _context.Users
            .GroupJoin(_context.UserRoles,
                user => user.Id,
                userRole => userRole.UserId,
                (user, userRoles) => new
                {
                    User = user, UserRoles = userRoles
                })
            .SelectMany(
                x => x.UserRoles.DefaultIfEmpty(),
                (user, userRole) =>
                    new
                    {
                        user.User, UserRole = userRole
                    })
            .GroupJoin(_context.Roles,
                userUserRole => userUserRole.UserRole.RoleId,
                role => role.Id,
                (
                        userUserRole,
                        roles) =>
                    new UserWithRolesViewModel
                    {
                        AppUser = userUserRole.User,
                        Role = userUserRole.UserRole,
                        Roles = roles.Select(role => role.Name).ToList()
                    })
            .AsNoTracking()
            .ToList();

        return usersWithRoles;
    }


    private List<UserWithRolesViewModel> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<UserWithRolesViewModel> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session
            .TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            return JsonConvert.DeserializeObject<
                       List<UserWithRolesViewModel>>(json) ??
                   new List<UserWithRolesViewModel>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = GetUsersWithRolesList();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 = PaginationViewModel<UserWithRolesViewModel>
            .StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

        return recordsQuery;
    }


    // GET: Users
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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
    public IActionResult IndexCards(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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
    public IActionResult IndexCards1(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery =
            SessionData<UserWithRolesViewModel>();

        var model =
            new PaginationViewModel<UserWithRolesViewModel>(
                recordsQuery,
                pageNumber, pageSize,
                recordsQuery.Count,
                sortOrder, sortProperty
            );

        return View(model);
    }


    // GET: AppUsers/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        var appUser = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (appUser == null)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        return View(appUser);
    }


    // GET: AppUsers/Create
    /// <summary>
    ///     Create action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        // ViewData["SubscriptionId"] = new SelectList(
        //     _context.Subscriptions,
        //     nameof(Subscription.Id),
        //     nameof(Subscription.Details)
        // );

        return View();
    }

    // POST: AppUsers/Create
    //
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create action.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        // [Bind(
        //     "FirstName,LastName,Address,SubscriptionId,ImageId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")]
        AppUser appUser)
    {
        if (!ModelState.IsValid) return View(appUser);

        _context.Add(appUser);

        await _context.SaveChangesAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: AppUsers/Edit/5
    /// <summary>
    ///     Edit action.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        var appUser = await _context.Users.FindAsync(id);

        if (appUser == null)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        // ViewData["SubscriptionId"] = new SelectList(
        //     _context.Subscriptions,
        //     nameof(Subscription.Id),
        //     nameof(Subscription.Details),
        //     appUser.SubscriptionId
        // );

        return View(appUser);
    }


    // POST: AppUsers/Edit/5
    //
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="appUser"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        // [Bind(
        //     "FirstName,LastName,Address,SubscriptionId,ImageId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")]
        AppUser appUser)
    {
        if (id != appUser.Id)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        if (!ModelState.IsValid)
            return View(appUser);

        // ViewData["SubscriptionId"] = new SelectList(
        //     _context.Subscriptions,
        //     nameof(Subscription.Id),
        //     nameof(Subscription.Details),
        //     appUser.SubscriptionId
        // );

        try
        {
            _context.Update(appUser);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.Id == id))
                return new NotFoundViewResult(nameof(UserNotFound),
                    CurrentClass,
                    id, CurrentController, nameof(Index));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: AppUsers/Delete/5
    /// <summary>
    ///     Delete action.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        var appUser = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (appUser == null)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        return View(appUser);
    }


    // POST: AppUsers/Delete/5
    /// <summary>
    ///     DeleteConfirmed action.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var appUser = await _context.Users.FindAsync(id);

        if (appUser == null)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        try
        {
            _context.Users.Remove(appUser);

            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
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
                        $"The {nameof(AppUser)} with the ID " +
                        $"{appUser.Id} - {appUser.FullName} " +
                        // $"{reservation.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(AppUser),
                    ItemId = appUser.Id,
                    ItemGuid = Guid.Empty,
                    ItemName = appUser.FullName
                };

                // Redirecione para o DatabaseError com os dados apropriados
                return RedirectToAction(
                    "DatabaseError", "Errors", dbErrorViewModel);
            }

            dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Database Error",
                ErrorMessage = "An error occurred while deleting the entity.",
                ItemClass = nameof(AppUser),
                ItemId = appUser.Id,
                ItemGuid = Guid.Empty,
                ItemName = appUser.FullName
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (InvalidOperationException ex)
        {
            // Handle the exception
            Console.WriteLine("An InvalidOperationException occurred: " +
                              ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(AppUser)} with the ID " +
                    $"{appUser.Id} - {appUser.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = appUser.Id,
                ItemGuid = Guid.Empty,
                ItemName = appUser.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (MySqlException ex)
        {
            // Catch any other exceptions that might occur
            Console.WriteLine("An error occurred: " + ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(AppUser)} with the ID " +
                    $"{appUser.Id} - {appUser.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = appUser.Id,
                ItemGuid = Guid.Empty,
                ItemName = appUser.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (Exception ex)
        {
            // Catch any other exceptions that might occur
            Console.WriteLine("An error occurred: " + ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(AppUser)} with the ID " +
                    $"{appUser.Id} - {appUser.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = appUser.Id,
                ItemGuid = Guid.Empty,
                ItemName = appUser.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    // ---------------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // ---------------------------------------------------------------------- //
}