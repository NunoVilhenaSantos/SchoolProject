using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
using SchoolProject.Web.Models.Account;
using SchoolProject.Web.Models.Errors;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     UsersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class UsersController : Controller
{
    // Obtém o tipo da classe atual
    // Obtém o nome do controlador atual
    internal const string CurrentClass = nameof(UserWithRolesViewModel);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string CurrentAction = nameof(Index);
    internal const string ClassRole = CurrentClass;

    internal const string SortProperty =
        nameof(UserWithRolesViewModel.AppUser.FullName);

    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // datacontexts
    private readonly DataContextMySql _context;


    // Helpers
    private readonly IConverterHelper _converterHelper;

    // host environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;

    // private readonly DataContextMySql _contextMySql;
    private readonly UserManager<AppUser> _userManager;


    /// <summary>
    ///     UsersController constructor.
    /// </summary>
    /// <param name="mailHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="context"></param>
    /// <param name="userManager"></param>
    /// <param name="authenticatedUserInApp"></param>
    public UsersController(
        IMailHelper mailHelper,
        IUserHelper userHelper,
        IStorageHelper storageHelper,
        IConverterHelper converterHelper,
        IWebHostEnvironment hostingEnvironment,
        DataContextMySql context,
        UserManager<AppUser> userManager,
        AuthenticatedUserInApp authenticatedUserInApp)
    {
        _hostingEnvironment = hostingEnvironment;
        _userManager = userManager;
        _authenticatedUserInApp = authenticatedUserInApp;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _mailHelper = mailHelper;
        _userHelper = userHelper;
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

        HttpContext.Session.Remove(SessionVarName);

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
            if (!_context.Users.Any(e => e.Id == id))
                return new NotFoundViewResult(
                    nameof(UserNotFound), CurrentClass, id,
                    CurrentController, nameof(Index));
            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

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

        if (user == null)
            return new NotFoundViewResult(nameof(UserNotFound), CurrentClass,
                id, CurrentController, nameof(Index));

        try
        {
            _context.Users.Remove(user);

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
                        $"{user.Id} - {user.FullName} " +
                        // $"{reservation.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(AppUser),
                    ItemId = user.Id,
                    ItemGuid = Guid.Empty,
                    ItemName = user.FullName
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
                ItemId = user.Id,
                ItemGuid = Guid.Empty,
                ItemName = user.FullName
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
                    $"{user.Id} - {user.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = user.Id,
                ItemGuid = Guid.Empty,
                ItemName = user.FullName
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
                    $"{user.Id} - {user.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = user.Id,
                ItemGuid = Guid.Empty,
                ItemName = user.FullName
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
                    $"{user.Id} - {user.FullName} " +
                    // $"{reservation.IdGuid} +" +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(AppUser),
                ItemId = user.Id,
                ItemGuid = Guid.Empty,
                ItemName = user.FullName
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    // -------------------------------------------------------------- //


    // POST: Users/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, to create a new appUser.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppUserViewModel model)
    {
        if (!ModelState.IsValid)
            ModelState.AddModelError(string.Empty,
                "The model is not valid. " +
                "You need to fill all the required fields.");


        var user = _converterHelper.ViewModelToUser(model, true);

        if (model.ImageFile is {Length: > 0})
            user.ProfilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    model.ImageFile, BucketName);

        var addUser = await _userHelper.AddUserAsync(user);

        if (addUser.Succeeded)
        {
            await _userHelper.AddUserToRoleAsync(user, model.Role);

            var token =
                await _userHelper.GeneratePasswordResetTokenAsync(user);

            var tokenUrl = Url.Action(
                nameof(AccountController.ResetPassword),
                "Account",
                new {token},
                HttpContext.Request.Scheme);

            if (!string.IsNullOrEmpty(tokenUrl))
            {
                var sendPasswordResetEmail =
                    _mailHelper.SendPasswordResetEmail(user, tokenUrl);

                if (sendPasswordResetEmail)
                {
                    TempData["Message"] =
                        $"An email has been sent to <i>{model.Email}</i> " +
                        $"with a link to reset password.";

                    // Success.
                    return RedirectToAction(nameof(Index));
                }
            }

            // If it gets here, rollback appUser creation.
            await _userHelper.DeleteUserAsync(user);
        }

        AddModelError("Could not create AppUser.");

        ViewBag.Roles = await _userHelper.GetComboRolesAsync();
        // ViewBag.Subscriptions =
        //     _subscriptionRepository.GetComboSubscriptions();

        return Content("" +
                       "<div class=\"flex items-center p-4 mb-4 text-sm text-red-800 border border-red-300 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400 dark:border-red-800\">\r\n" +
                       "" +
                       "<svg class=\"flex-shrink-0 inline w-4 h-4 mr-3\" aria-hidden=\"true\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" viewBox=\"0 0 20 20\">\r\n" +
                       "" + "" +
                       "<path d=\"M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z\" />\r\n" +
                       "" + "</svg>\r\n" +
                       "" + "<span class=\"sr-only\">Info</span>\r\n" +
                       "" + "<div>\r\n" +
                       "" + "" +
                       "<span class=\"font-medium\">Error!</span>\r\n" +
                       "" + "" +
                       "<span> AppUser could not be created</span>\r\n" +
                       "" + "</div>\r\n" +
                       "</div>",
            "text/html");
    }


    // POST: Users/Edit/5
    /// <summary>
    ///     Edit method, to edit a specific appUser.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, AppUserViewModel model)
    {
        if (!ModelState.IsValid)
            ModelState.AddModelError(string.Empty,
                "The model is not valid. " +
                "You need to fill all the required fields.");


        try
        {
            var user = await _userHelper.GetUserByIdAsync(model.Id);
            if (user == null)
                return new NotFoundViewResult(
                    nameof(UserNotFound), CurrentClass, id,
                    CurrentController, nameof(Index));


            var isEmailChanged = user.Email != model.Email;
            var oldEmail = user.Email;

            // Keep email confirmed if already confirmed and has not changed.
            if (user.EmailConfirmed && !isEmailChanged)
                model.EmailConfirmed = true;

            user = _converterHelper.ViewModelToUser(model, user, false);

            if (model.ImageFile != null)
            {
                user.ProfilePhotoId =
                    await _storageHelper.UploadStorageAsync(
                        model.ImageFile, BucketName);
            }
            else if (model.DeletePhoto)
            {
                await _storageHelper.DeleteStorageAsync(
                    user.ProfilePhotoId.ToString(), BucketName);

                user.ProfilePhotoId = Guid.Empty;
            }

            var updateUser = await _userHelper.UpdateUserAsync(user);
            if (updateUser.Succeeded)
            {
                if (!await _userHelper.IsUserInRoleAsync(user, model.Role))
                    await _userHelper.SetUserRoleAsync(user, model.Role);

                if (!isEmailChanged) return RedirectToAction(nameof(Index));

                var token = await _userHelper
                    .GenerateEmailConfirmationTokenAsync(user);

                var tokenUrl = Url.Action(
                    nameof(AccountController.ConfirmEmail),
                    "Account",
                    new
                    {
                        userid = user.Id,
                        token
                    },
                    HttpContext.Request.Scheme);

                if (!string.IsNullOrEmpty(tokenUrl))
                {
                    var sendConfirmationEmail =
                        _mailHelper.SendConfirmationEmail(user,
                            tokenUrl);

                    if (sendConfirmationEmail)
                    {
                        // Success.
                        TempData["Message"] =
                            "AppUser email address has changed." +
                            "A confirmation email has been sent to " +
                            $"<i>{user.Email}</i>";

                        return RedirectToAction(nameof(Index));
                    }
                }

                // Email address has changed but could not send reset password email.
                TempData["Message"] =
                    "Could not send confirmation email.";
                user.Email = oldEmail;

                var revertUserEmail =
                    await _userHelper.UpdateUserAsync(user);

                if (revertUserEmail.Succeeded)
                    TempData["Message"] +=
                        "<br />AppUser email has been reverted.";
                else
                    TempData["Message"] +=
                        "<br />Could not revert AppUser email.";

                // Success.
                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _userHelper.UserExistsAsync(model.Id))
                return new NotFoundViewResult(
                    nameof(UserNotFound), CurrentClass, id,
                    CurrentController, nameof(Index));

            throw;
        }

        AddModelError("Could not update AppUser.");

        ViewBag.Roles = await _userHelper.GetComboRolesAsync();
        // ViewBag.Subscriptions = _subscriptionRepository.GetComboSubscriptions();

        return Content("" +
                       "<div class=\"flex items-center p-4 mb-4 text-sm text-red-800 border border-red-300 rounded-lg bg-red-50 dark:bg-gray-800 dark:text-red-400 dark:border-red-800\">\r\n" +
                       "" +
                       "<svg class=\"flex-shrink-0 inline w-4 h-4 mr-3\" aria-hidden=\"true\" xmlns=\"http://www.w3.org/2000/svg\" fill=\"currentColor\" viewBox=\"0 0 20 20\">\r\n" +
                       "" + "" +
                       "<path d=\"M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM9.5 4a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3ZM12 15H8a1 1 0 0 1 0-2h1v-3H8a1 1 0 0 1 0-2h2a1 1 0 0 1 1 1v4h1a1 1 0 0 1 0 2Z\" />\r\n" +
                       "" + "</svg>\r\n" +
                       "" + "<span class=\"sr-only\">Info</span>\r\n" +
                       "" + "<div>\r\n" +
                       "" + "" +
                       "<span class=\"font-medium\">Error!</span>\r\n" +
                       "" + "" +
                       "<span> AppUser could not be updated</span>\r\n" +
                       "" + "</div>\r\n" +
                       "</div>",
            "text/html");
    }


    // public async Task<IActionResult> Admins()
    // {
    //     var models =
    //         await GetModelsForUsersInRoleAsync("Admin");
    //
    //     return View(nameof(Index2), models);
    // }

    // public async Task<IActionResult> Customers()
    // {
    //     var models =
    //         await GetModelsForUsersInRoleAsync("Customer");
    //
    //     return View(nameof(Index2), models);
    // }

    // public async Task<IActionResult> Librarians()
    // {
    //     var models =
    //         await GetModelsForUsersInRoleAsync("Librarian");
    //
    //     return View(nameof(Index2), models);
    // }

    // public async Task<IActionResult> Employees()
    // {
    //     var models =
    //         await GetModelsForUsersInRoleAsync("Employee");
    //
    //     return View(nameof(Index2), models);
    // }


    private async Task<IEnumerable<AppUserViewModel>>
        GetModelsForUsersInRoleAsync(string roleName)
    {
        var users = await _userHelper.GetUsersInRoleAsync(roleName);

        var models = new List<AppUserViewModel>();

        foreach (var user in users)
            models.Add(await GetUserViewModelForViewAsync(user));

        return models.OrderBy(m => m.Email);
    }


    private async Task<AppUserViewModel> GetUserViewModelForViewAsync(
        AppUser appUser)
    {
        var userRole = await _userHelper.GetUserRoleAsync(appUser);

        var model = _converterHelper.UserToViewModel(appUser, userRole);

        return model;
    }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}