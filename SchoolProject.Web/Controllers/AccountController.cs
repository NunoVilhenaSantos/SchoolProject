using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Data.Repositories.Genders;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Images;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Account;
using Sentry;

namespace SchoolProject.Web.Controllers;

/// <summary>
/// </summary>
public class AccountController : Controller
{
    // Obtém o tipo da classe atual
    internal const string ClassRole = CurrentClass;

    // Obtém o tipo da classe atual
    internal const string CurrentClass = UsersController.CurrentClass;
    internal const string CurrentAction = UsersController.CurrentAction;
    internal const string SessionVarName = UsersController.SessionVarName;
    internal const string SortProperty = UsersController.SortProperty;
    internal static readonly string BucketName = UsersController.BucketName;
    private readonly ICityRepository _cityRepository;
    private readonly IConfiguration _configuration;


    // helpers
    private readonly IConverterHelper _converterHelper;


    // repositories
    private readonly ICountryRepository _countryRepository;
    private readonly IGenderRepository _genderRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;


    // auxiliaries
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IImageHelper _imageHelper;
    private readonly ILogger<AccountController> _logger;
    private readonly IMailHelper _mailHelper;
    private readonly RoleManager<IdentityRole> _roleManager;

    // private readonly ICombosHelper _combosHelper;
    private readonly IHub _sentryHub;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IStorageHelper _storageHelper;
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUserHelper _userHelper;


    /// <summary>
    /// </summary>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="configuration"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="sentryHub"></param>
    /// <param name="storageHelper"></param>
    /// <param name="logger"></param>
    /// <param name="imageHelper"></param>
    /// <param name="countryRepository"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="cityRepository"></param>
    /// <param name="genderRepository"></param>
    /// <param name="converterHelper"></param>
    /// <param name="studentRepository"></param>
    /// <param name="teacherRepository"></param>
    public AccountController(
        IUserHelper userHelper, IMailHelper mailHelper,
        IConfiguration configuration, IWebHostEnvironment hostingEnvironment,
        IHub sentryHub, IStorageHelper storageHelper,
        ILogger<AccountController> logger, IImageHelper imageHelper,
        ICountryRepository countryRepository,
        IHttpContextAccessor httpContextAccessor,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ICityRepository cityRepository, IGenderRepository genderRepository,
        IConverterHelper converterHelper, IStudentRepository studentRepository,
        ITeacherRepository teacherRepository)
    {
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
        _sentryHub = sentryHub;
        _storageHelper = storageHelper;
        _logger = logger;
        _imageHelper = imageHelper;
        _countryRepository = countryRepository;
        _httpContextAccessor = httpContextAccessor;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _cityRepository = cityRepository;
        _genderRepository = genderRepository;
        _converterHelper = converterHelper;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(AccountController));


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

    // ------------------------------- ------ ------------------------------- //

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public async Task<IActionResult> Index()
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var user = await _userHelper.GetUserByEmailAsync(GetCurrentUserName());

        if (user == null)
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, user?.Id,
                CurrentController, nameof(Index));

        var model = new UpdateAppUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            HasPhoto = user.ProfilePhotoId != Guid.Empty,
            ProfilePhotoId = user.ProfilePhotoId,
            Role = await _userHelper.GetUserRoleAsync(user),
            WasDeleted = false
            // CityId = user.CityId,
            // City = user.City,
        };

        return View(model);
    }


    /// <summary>
    ///     Aqui o utilizador faz o login no sistema e é direcionado para a página inicial
    /// </summary>
    /// <returns></returns>
    public IActionResult Login()
    {
        if (User.Identity is {IsAuthenticated: true})
            return RedirectToAction(nameof(Index), "Home");

        return View();
    }


    // Aqui é que de fato valida as informações
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(
                string.Empty, "Failed to login!");
            return View(model);
        }

        var result = await _userHelper.LoginAsync(model);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(
                string.Empty, "Failed to login!");
            return View(model);
        }

        if (Request.Query.Keys.Contains("ReturnUrl"))
            return Redirect(Request.Query["ReturnUrl"].First());

        // Caso tente acessar outra view diferente do Login,
        // sou direcionado para a view Login, mas após sou direcionado
        // para a view que tentei acessar em primeiro lugar.
        //
        // Exemplo: ProductsController [Authorize]
        //

        return RedirectToAction(nameof(Index), "Home");
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Logout()
    {
        await _userHelper.LogoutAsync();

        return RedirectToAction(nameof(Index), "Home");
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin,SuperUser,Functionary")]
    public IActionResult Register()
    {
        var model = new RegisterNewAppUserViewModel
        {
            FirstName = null,
            LastName = null,
            UserName = null,
            Email = null,
            Address = null,
            PhoneNumber = null,

            Password = null,
            ConfirmPassword = null,

            WasDeleted = false,
            ProfilePhotoId = default,

            CountriesList =
                _countryRepository.GetComboCountriesAndNationalities(),
            CountryId = null,

            CitiesList = new List<SelectListItem>(),
            CityId = null,

            RolesList = new SelectList(_roleManager.Roles,
                nameof(IdentityRole.Id),
                nameof(IdentityRole.Name), 0),

            RoleId = null,

            GendersList = _genderRepository.GetAll()
                .Select(g => new SelectListItem
                {
                    Text = g.Name,
                    Value = g.Id.ToString()
                })
                .OrderBy(g => g.Text),
            GenderId = null
        };

        FillViewLists();

        return View(model);
    }


    // Aqui é que de fato valida as informações
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewAppUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.UserName == null)
            ModelState.AddModelError(
                string.Empty, "The AppUser has no username.");

        model.UserName = model.Email;

        var user = await _userHelper.GetUserByEmailAsync(model.UserName);

        if (user != null)
            ModelState.AddModelError(
                string.Empty, "The user is already register.");


        var profilePhotoId = model.ProfilePhotoId;

        if (model.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    model.ImageFile, BucketName);

        model.ProfilePhotoId = profilePhotoId;


        user = new AppUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            PhoneNumber = model.PhoneNumber,
            Email = model.UserName,
            UserName = model.UserName,
            ProfilePhotoId = model.ProfilePhotoId,
            WasDeleted = false
            // Country = model.Country,
            // CityId = model.CityId,
            // City = model.City,
        };


        // AQUI QUE DEVE SER COLOCADO OS OUTROS ROLES (VIDEO 19 - 29')

        var result =
            await _userHelper.AddUserAsync(user, model.Password);

        if (result != IdentityResult.Success)
        {
            ModelState.AddModelError(string.Empty,
                "The AppUser couldn't be created");
            return View(model);
        }

        var role = await _roleManager.FindByIdAsync(model.RoleId);

        await _userHelper.AddUserToRoleAsync(user, role?.Name);


        // ***** criação do Student or teacher via Account/Register *******


        switch (role.Name)
        {
            case StudentsController.ClassRole:
                try
                {
                    var student = _converterHelper.ToStudentFromUser(user);

                    await _studentRepository.CreateAsync(student);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty,
                        "An Error has occurred. Try again later.");
                    return View(model);
                }

                break;


            case TeachersController.ClassRole:
                try
                {
                    var teacher = _converterHelper.ToTeacherFromUser(user);

                    await _teacherRepository.CreateAsync(teacher);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty,
                        "An Error has occurred. Try again later.");
                    return View(model);
                }

                break;
        }


        // ***** FIM *** criação do Student or teacher via Account/Register ***


        // ***** Incluído por causa do Email de Confirmação *******
        var myToken =
            await _userHelper.GenerateEmailConfirmationTokenAsync(user);


        var tokenLink = Url.Action(
            "ConfirmEmail", "Account",
            new
            {
                userId = user.Id, token = myToken
            },
            HttpContext.Request.Scheme);


        var response = _mailHelper.SendEmail(model.UserName,
            "Email confirmation",
            "<h2>Email Confirmation<h2>" +
            "To allow the appUser, " +
            "please click in this link:" +
            "</br></br>" +
            $"<a href = \"{tokenLink}\">Confirm Email</a>" +
            "</br></br>" +
            $"<p>Password temporária: {model.Password}</a>");

        //var response = _mailHelper.SendEmail1(model.UserName,
        //    "Email confirmation",
        //    "<h2>Email Confirmation<h2>" +
        //    "To allow the appUser, " +
        //    "please click in this link:" +
        //    "</br></br>" +
        //    $"<a href = \"{tokenLink}\">Confirm Email</a>" +
        //    "</br></br>" +
        //    $"<p>Password temporária: {model.Password}</a>");

        if (response.IsSuccess)
        {
            ViewBag.Message =
                "The instructions to allow you appUser has been sent to email";

            return View(model);
        }


        ModelState.AddModelError(
            string.Empty, "The AppUser couldn't be logged");


        //*****Retirado por causa do Email de Confirmação*********
        //var loginViewModel = new LoginViewModel
        //{
        //    UserName = model.UserName,
        //    Password = model.Password,
        //    RememberMe = false,
        //};

        //var result2 = await _userHelper.LoginAsync(loginViewModel);

        return View(model);
    }

    //Todo REGISTER NOVO
    /*[HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        NIF = model.NIF,
                        PostalCode = model.PostalCode,
                        Location = model.Location,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    var role = await _userHelper.GetRoleNameByIdAsync(model.RoleId);

                    await _userHelper.AddUserToRoleAsync(user, role.Name);

                    if (role.Name == "Customer")
                    {
                        try
                        {
                            var customer = _converterHelper.ToCustomerFromUser(user);

                            await _customerRepository.CreateAsync(customer);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(string.Empty, "An Error has occurred. Try again later.");
                            return View(model);
                        }
                    }
                    if (role.Name == "Mechanic")
                    {
                        try
                        {
                            var mechanic = _converterHelper.ToMechanicFromUser(user);

                            await _mechanicRepository.CreateAsync(mechanic);
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(string.Empty, "An Error has occurred. Try again later.");
                            return View(model);
                        }
                    }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"please click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a> <p><p/><p>Temp Password: {model.Password}<p/>");


                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow your user have been sent to your email";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");
                }
            }

            return View(model);
    }*/


    // Aqui só aparece a View
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> ChangeUser()
    {
        var user = await _userHelper.GetUserByEmailWithCity(User.Identity.Name)
            .Result.FirstOrDefaultAsync();

        if (user == null)
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, user.Id, CurrentController,
                nameof(Index));

        var model = new ChangeAppUserViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            WasDeleted = user.WasDeleted,
            ProfilePhotoId = user.ProfilePhotoId

            // CountryId = user.City.Country.Id,
            // CountriesList =
            //     _countryRepository.GetComboCountriesAndNationalities(),
            // City = user.City,
            // CityId = user.City.Id,
            // CitiesList =
            //     _countryRepository.GetComboCities(user.City.Country.Id),
        };

        return View(model);
    }


    // Aqui é que de fato valida as informações
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangeUser(ChangeAppUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user =
            await _userHelper.GetUserByEmailAsync(User.Identity.Name);

        if (user == null) return View(model);


        var profilePhotoId = model.ProfilePhotoId;

        if (model.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    model.ImageFile, BucketName);

        model.ProfilePhotoId = profilePhotoId;


        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        // user.UserName = model.UserName;
        // user.Email = model.Email;
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;
        user.ProfilePhotoId = model.ProfilePhotoId;

        // user.CountryId = model.City.Country.Id;
        // user.Country = model.City.Country;
        // user.CityId = model.CityId;
        // user.City = _cityRepository.GetByIdAsync(model.CityId).Result;


        var response = await _userHelper.UpdateUserAsync(user);

        if (response.Succeeded)
            ViewBag.UserMessage = "AppUser Updated!";
        else
            ModelState.AddModelError(string.Empty,
                response.Errors.FirstOrDefault()?.Description);

        return View(model);
    }


    // Aqui só aparece a View
    //"Botão Direito" -> AddView
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult ChangePassword()
    {
        return View();
    }


    // Aqui é que de fato valida as informações
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user =
            await _userHelper.GetUserByEmailAsync(User.Identity.Name);

        switch (user)
        {
            case null:
                ModelState.AddModelError(
                    string.Empty, "AppUser not found.");
                return View(model);

            default:
            {
                var result = await _userHelper.ChangePasswordAsync(
                    user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                    return RedirectToAction(nameof(ChangeUser));

                // Aparece msg caso a senha antiga esteja incorreta
                ModelState.AddModelError(string.Empty,
                    result.Errors.FirstOrDefault().Description);
                break;
            }
        }

        return View(model);
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateToken(
        [FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await _userHelper.GetUserByEmailAsync(model.Username);

        if (user == null) return BadRequest();

        var result =
            await _userHelper.ValidatePasswordAsync(user, model.Password);

        if (!result.Succeeded) return BadRequest();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email), new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

        var credentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Tokens:Issuer"],
            _configuration["Tokens:Audience"],
            claims,
            expires: DateTime.UtcNow
                .AddDays(15), // Tempo validade Token
            signingCredentials: credentials);

        var results = new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        };

        return Created(string.Empty, results);
    }


    /// <summary>
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, userId,
                CurrentController, nameof(Index));


        var user = await _userHelper.GetUserByIdAsync(userId);

        if (user == null)
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, userId,
                CurrentController, nameof(Index));


        var result = await _userHelper.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
            return new NotFoundViewResult(
                nameof(UserNotFound), CurrentClass, userId,
                CurrentController, nameof(Index));


        return View();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult RecoverPassword()
    {
        return View();
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RecoverPassword(
        RecoverPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userHelper.GetUserByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty,
                "The email doesn't correspond to a registered appUser.");
            return View(model);
        }

        var myToken =
            await _userHelper.GeneratePasswordResetTokenAsync(user);

        var link = Url.Action(
            "ResetPassword",
            "Account",
            new {token = myToken}, HttpContext.Request.Scheme);

        var response = _mailHelper.SendEmail(model.Email,
            "Account Password Reset",
            "<h2>Account Password Reset</h2>" +
            "To reset the password click in this link:</br></br>" +
            $"<a href = \"{link}\">Reset Password</a>");

        if (response.IsSuccess)
            ViewBag.Message =
                "The instructions to recover your password has been sent to email.";

        return View();
    }


    /// <summary>
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public IActionResult ResetPassword(string token)
    {
        return View();
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var user = await _userHelper.GetUserByEmailAsync(model.UserName);

        if (user == null)
        {
            ViewBag.Message = "AppUser not found...";
            return View(model);
        }

        // Aqui que fazemos o reset
        var result = await _userHelper.ResetPasswordAsync(
            user, model.Token, model.Password);

        if (result.Succeeded)
        {
            ViewBag.Message = "Password reset successfully.";
            return View();
        }

        ViewBag.Message = "Error while resetting the password";

        return View(model);
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult ForgotPassword()
    {
        return IsUserAuthenticated() ? RedirectToHomePage() : View();
    }


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(
        ForgotPasswordViewModel model)
    {
        if (IsUserAuthenticated()) return RedirectToHomePage();

        if (!ModelState.IsValid) return View(model);


        var user = await _userHelper.GetUserByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty,
                "The email doesn't correspond to a registered appUser.");
            return View(model);
        }

        var token =
            await _userHelper.GeneratePasswordResetTokenAsync(user);

        var tokenUrl = Url.Action(
            nameof(ResetPassword),
            "Account",
            new {token},
            HttpContext.Request.Scheme);

        if (!string.IsNullOrEmpty(tokenUrl))
        {
            var sendPasswordResetEmail =
                _mailHelper.SendPasswordResetEmail(user, tokenUrl);
            if (sendPasswordResetEmail)
            {
                // Success.

                TempData["Message"] =
                    $"An email has been sent to <i>{model.Email}</i> " +
                    $"with a link to reset password.";

                return RedirectToHomePage();
            }
        }

        ModelState.AddModelError(
            string.Empty, "Could not send password reset email.");

        return View(nameof(ForgotPassword), model);
    }


    private IActionResult RedirectToHomePage()
    {
        return Redirect("/Home");
    }


    private string GetCurrentUserName()
    {
        return User.Identity?.Name ?? "";
    }


    private bool IsUserAuthenticated()
    {
        return User.Identity?.IsAuthenticated ?? false;
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult NotAuthorized()
    {
        return View();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult UserNotFound()
    {
        return View();
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Aqui o utilizador obtém a lista de cidades de um determinado pais
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetCitiesByCountryJson")]
    [Route("Account/GetCitiesByCountryJson")]
    public async Task<JsonResult> GetCitiesByCountryJson(int countryId)
    {
        if (countryId == 0) return Json(new List<City>());

        var country = await _countryRepository
            .GetCountryWithCitiesAsync(countryId)
            .FirstOrDefaultAsync();

        // Serialize the country object to JSON with ReferenceHandler.Preserve
        var countryJson = JsonSerializer.Serialize(country,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler =
                    ReferenceHandler.Preserve
            });

        // Print the JSON representation to the console
        Console.WriteLine(countryJson);

        var cities1 =
            _countryRepository.GetComboCities(countryId);

        return Json(cities1);
    }


    /// <summary>
    ///     Aqui o utilizador obtém a lista de países que existem no sistema
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetCountriesJson")]
    [Route("Account/GetCountriesJson")]
    public Task<JsonResult> GetCountriesJson()
    {
        var country =
            _countryRepository.GetCountriesWithCitiesEnumerable();


        return Task.FromResult(Json(country.OrderBy(c => c.Name)));
    }


    /// <summary>
    ///     Aqui o utilizador obtém a nacionalidade desse país
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetNationalitiesJson")]
    [Route("Account/GetNationalitiesJson")]
    public Task<JsonResult> GetNationalitiesJson(int countryId)
    {
        var nationalities =
            _countryRepository.GetComboNationalities(countryId);

        return Task.FromResult(
            Json(nationalities.OrderBy(c => c.Text)));

        // return Task.FromResult(Json(nationalities));
    }


    /// <summary>
    ///     Aqui o utilizador obtém a lista de países e a respetiva nacionalidade
    ///     via JSON para o preenchimento do dropdown-list
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Account/GetCountriesWithNationalitiesJson")]
    [Route("Account/GetCountriesWithNationalitiesJson")]
    public Task<JsonResult> GetCountriesWithNationalitiesJson()
    {
        var countriesWithNationalities =
            _countryRepository.GetComboCountriesAndNationalities();

        return Task.FromResult(
            Json(countriesWithNationalities
                .OrderBy(c => c.Text)));
    }


    // ---------------------------------------------------------------------- //


    private void FillViewLists(int countryId = 0, int cityId = 0,
        int genderId = 0, string? roleId = null
    )
    {
        ViewData[nameof(Student.CountryId)] =
            new SelectList(_countryRepository.GetAll()
                    .Include(w => w.Nationality)
                    .OrderBy(e => e.Name).ToList(),
                nameof(Country.Id),
                $"{nameof(Country.Name)} ({nameof(Country.Nationality.Name)})",
                countryId);

        ViewData[nameof(Student.CityId)] =
            new SelectList(
                _cityRepository.GetAll()
                    .Include(e => e.Country)
                    .OrderBy(e => e.Name).ToList(),
                $"{nameof(City.Id)} ({nameof(City.Country.Name)})",
                nameof(City.Name),
                cityId);

        ViewData[nameof(Student.GenderId)] =
            new SelectList(_genderRepository.GetAll(),
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                genderId);

        ViewData["RoleId"] =
            new SelectList(_roleManager.Roles,
                nameof(IdentityRole.Id),
                nameof(IdentityRole.Name),
                roleId);

        Console.WriteLine("Debug zone!!!");
    }


    // ---------------------------------------------------------------------- //

    // Adicione um método para calcular o tempo restante para o token expirar.
    private TimeSpan GetTimeRemaining(DateTime expirationDate)
    {
        return expirationDate - DateTime.UtcNow;
    }


    // ---------------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }


    // ---------------------------------------------------------------------- //
}