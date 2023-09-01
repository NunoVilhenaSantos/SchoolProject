using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Images;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Account;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
/// Account Controller for adding, editing, deleting, change password
/// </summary>
public class AccountController : Controller
{
    private const string BucketName = "users";

    private readonly ICountryRepository _countryRepository;
    private readonly IConfiguration _configuration;
    private readonly IStorageHelper _storageHelper;
    private readonly IE_MailHelper _emailHelper;
    private readonly IImageHelper _imageHelper;
    private readonly IUserHelper _userHelper;


    /// <summary>
    /// Constructor for the Account controller
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="configuration"></param>
    /// <param name="storageHelper"></param>
    /// <param name="emailHelper"></param>
    /// <param name="imageHelper"></param>
    /// <param name="userHelper"></param>
    public AccountController(
        ICountryRepository countryRepository,
        IConfiguration configuration,
        IStorageHelper storageHelper,
        IE_MailHelper emailHelper,
        IImageHelper imageHelper,
        IUserHelper userHelper
    )
    {
        _userHelper = userHelper;
        _imageHelper = imageHelper;
        _emailHelper = emailHelper;
        _storageHelper = storageHelper;
        _configuration = configuration;
        _countryRepository = countryRepository;
    }


    /// <summary>
    /// Aqui o utilizador é reencaminhado para a view de Login caso não esteja autenticado
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity is {IsAuthenticated: true})
            return RedirectToAction("Index", "Home");

        return View();
    }


    /// <summary>
    /// Aqui é que se valida as informações do utilizador
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userHelper.LoginAsync(model);

            if (result.Succeeded)
                //
                // Caso tente acessar outra view diferente do
                // Login sou direcionado para a view Login,
                //
                // mas após sou direcionado para a view
                // que tentei acessar em primeiro lugar.
                //
                // Exemplo: ProductsController [Authorize]
                //
                return Request.Query.Keys.Contains("ReturnUrl")
                    ? Redirect(Request.Query["ReturnUrl"].First() ??
                               string.Empty)
                    : RedirectToAction("Index", "Home");

            ModelState.AddModelError(
                string.Empty, "Failed to login!");
            return View(model);
        }

        ModelState.AddModelError(
            string.Empty, "Failed to login!");
        return View(model);
    }


    /// <summary>
    /// Aqui o utilizador faz o logout da sua conta
    /// Aqui o utilizador é reencaminhado para a view Index do controlador Home
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _userHelper.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }


    /// <summary>
    /// Aqui o utilizador é reencaminhado para a view Register para criar uma nova conta
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin,SuperUser")]
    public IActionResult Register()
    {
        var model = new RegisterNewUserViewModel
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            UserName = string.Empty,
            Password = string.Empty,
            ConfirmPassword = string.Empty,
            WasDeleted = false,
            ProfilePhotoId = default,

            CountryId = 0,
            Countries = _countryRepository
                .GetComboCountriesAndNationalities(),
            CityId = 0,
            Cities = _countryRepository.GetComboCities(0)
        };


        return View(model);
    }


    /// <summary>
    ///     Aqui é que se valida as informações do utilizador para a criação de uma nova conta
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterNewUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.UserName != null)
            {
                var user =
                    await _userHelper.GetUserByEmailAsync(model.UserName);

                if (user == null)
                {
                    // var city = await
                    //     _countryRepository.GetCityAsync(model.CityId);
                    // var country = await
                    //     _countryRepository.GetCountryAsync(model.CountryId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.UserName,
                        UserName = model.UserName,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        WasDeleted = false,
                        ProfilePhotoId = model.ImageFile is null
                            ? default
                            : _storageHelper.UploadFileAsyncToGcp(
                                model.ImageFile, BucketName).Result
                        // City = city,
                        // CityId = city.Id,
                        // Country = country,
                        // CountryId = country.Id,
                        // NationalityId = country.Nationality.Id,
                    };


                    var result =
                        await _userHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "The user couldn't be created.");
                        return View(model);
                    }


                    // var loginViewModel = new LoginViewModel
                    // {
                    //     Password = model.Password,
                    //     RememberMe = false,
                    //     Username = model.Username
                    // };

                    // var result2 = await _userHelper.LoginAsync(loginViewModel);
                    // if (result2.Succeeded)
                    //     return RedirectToAction("Index", "Home");


                    var myToken =
                        await _userHelper.GenerateEmailConfirmationTokenAsync(
                            user);

                    var tokenLink = Url.Action("ConfirmEmail",
                        "Account",
                        new
                        {
                            userid = user.Id,
                            token = myToken
                        },
                        HttpContext.Request.Scheme
                    );


                    var response = await _emailHelper.SendEmailAsync(
                        model.UserName,
                        "Email confirmation",
                        $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"please click in this link:" +
                        $"</br></br><a href = \"{tokenLink}\">" +
                        $"Confirm Email</a></br><p>Temporary Password: " +
                        $"{model.Password}</p>");


                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to verify the " +
                                          "account have been sent to email";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty,
                        "The User couldn't be logged.");
                    return View(model);
                }
            }

            ModelState.AddModelError(
                string.Empty, "User already exists.");

            return View(model);
            // return RedirectToAction("Login", "Account");
        }

        ModelState.AddModelError(
            string.Empty,
            "Tem de preencher os campos, obrigatórios!");


        return View(model);
    }


    /// <summary>
    /// Aqui o utilizador faz as alterações aos seus dados da sua conta
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ChangeUser()
    {
        if (User.Identity?.Name is null) return View();

        var user =
            await _userHelper.GetUserByEmailAsync(User.Identity.Name);

        if (user == null) return View();

        var model = new ChangeUserViewModel
        {
            CountryId = 0,
            Countries = _countryRepository
                .GetComboCountriesAndNationalities(),
            CityId = 0,
            Cities = _countryRepository.GetComboCities(0),

            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,

            WasDeleted = user.WasDeleted,
            ProfilePhotoId = user.ProfilePhotoId
        };


        return View(model);
    }


    /// <summary>
    /// Aqui é que se alteram as informações do utilizador
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(
                string.Empty,
                "Failed to update user information!");

            // Return the view with the invalid model
            return View(model);
        }

        if (User.Identity?.Name == null) return View(model);

        var user =
            await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

        // Return the view, as user is not found
        if (user == null) return View(model);

        if (model.ImageFile != null)
        {
            var profilePhotoId =
                await _storageHelper.UploadFileAsyncToGcp(
                    model.ImageFile, BucketName);

            // var profilePhotoIdAzure =
            //     await _storageHelper.UploadStorageAsync(
            //         model.ImageFile, BucketName);

            // await _storageHelper.DeleteFileAsyncFromGcp(
            //     user.ProfilePhotoId.ToString(),
            //     BucketName);

            model.ProfilePhotoId = profilePhotoId;
            user.ProfilePhotoId = profilePhotoId;
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;
        user.WasDeleted = model.WasDeleted;
        user.ProfilePhotoId = model.ProfilePhotoId;

        // user.CityId = model.CityId;
        // user.CountryId = model.CountryId;
        // user.NationalityId = model.NationalityId;

        // user.Email = model.Username;

        var response = await _userHelper.UpdateUserAsync(user);

        if (response.Succeeded)
        {
            ViewBag.UserMessage = "User updated!";
        }
        else
        {
            var errorMessage =
                response.Errors.FirstOrDefault()?.Description;

            if (errorMessage != null)
                ModelState.AddModelError(string.Empty, errorMessage);
        }

        return View(model);
    }


    /// <summary>
    /// Aqui o utilizador faz as alterações da sua password
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }


    /// <summary>
    /// Aqui é que se altera a password do utilizador
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userHelper.GetUserByEmailAsync(User.Identity?.Name);

        if (user == null) return View();

        var response = await _userHelper.ChangePasswordAsync(
            user, model.OldPassword, model.NewPassword);

        if (response.Succeeded)
        {
            ViewBag.UserMessage = "Password changed!";
            return RedirectToAction("ChangeUser");
        }

        var errorMessage =
            response.Errors.FirstOrDefault()?.Description;
        if (errorMessage != null)
            ModelState.AddModelError(
                string.Empty, errorMessage);
        ModelState.AddModelError(
            string.Empty, "User not found.");


        return View(model);
    }


    // https://localhost:5001/Account/CreateToken
    // [Route("Account/CreateToken")]
    /// <summary>
    /// Aqui é que se cria o token para o utilizador validar a sua conta
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

        var result = await _userHelper.ValidatePasswordAsync(
            user, model.Password);
        if (!result.Succeeded) return BadRequest();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti,
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
            expires: DateTime.UtcNow.AddDays(15),
            signingCredentials: credentials);

        var results = new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        };

        return Created(string.Empty, results);
    }


    /// <summary>
    /// Aqui é que se confirma o email do utilizador
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            return NotFound();

        var user = await _userHelper.GetUserByIdAsync(userId);

        if (user == null) return NotFound();

        var result = await _userHelper.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
        }

        return View();
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // https://localhost:5001/Account/NotAuthorized
    /// <summary>
    ///     Aqui é que se mostra a view de NotAuthorized
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult NotAuthorized() => View();


    // https://localhost:5001/Account/AccessDenied
    /// <summary>
    ///    Aqui é que se mostra a view de AccessDenied
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }


    // https://localhost:5001/Account/Error
    /// <summary>
    ///     Aqui é que se mostra a view de Error
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Error() => View();


    /// <summary>
    ///    Aqui é que se mostra a vista para fazer a recuperação da password
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public IActionResult ResetPassword(string token) => View();


    /// <summary>
    ///   Aqui é que se mostra a vista para fazer a recuperação da password
    /// </summary>
    /// <returns></returns>
    public IActionResult RecoverPassword() => View();


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///    Aqui o utilizador obtém a lista de cidades de um determinado pais
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetCitiesAsync")]
    [Route("Account/GetCitiesAsync")]
    public async Task<JsonResult> GetCitiesAsync(int countryId)
    {
        if (countryId == 0) return Json(new List<City>());

        var country =
            await _countryRepository.GetCountryWithCitiesAsync(countryId);


        // Console.OutputEncoding = Encoding.UTF8;

        // Console.WriteLine(country);
        // Console.WriteLine(country?.Cities);
        // Console.WriteLine(Json(country?.Cities.OrderBy(c => c.Name)));

        // var cities = country.Cities.OrderBy(c => c.Name);
        // Console.WriteLine(cities);
        // Console.WriteLine(Json(cities));
        // Console.WriteLine(Json(cities.OrderBy(c => c.Name)));


        // Serialize the country object to JSON with ReferenceHandler.Preserve
        var countryJson = JsonSerializer.Serialize(country,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });

        // Print the JSON representation to the console
        Console.WriteLine(countryJson);

        var cities1 =
            _countryRepository.GetComboCities(countryId);

        return Json(cities1);
    }


    /// <summary>
    ///    Aqui o utilizador obtém a lista de países que existem no sistema
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetCountriesAsync")]
    [Route("Account/GetCountriesAsync")]
    public Task<JsonResult> GetCountriesAsync()
    {
        var country =
            _countryRepository.GetCountriesWithCitiesEnumerable();

        // Console.OutputEncoding = Encoding.UTF8;
        // Console.WriteLine(country);
        // Console.WriteLine(
        //     Json(country.OrderBy(c => c.Name)));

        return Task.FromResult(Json(country.OrderBy(c => c.Name)));
    }


    /// <summary>
    ///    Aqui o utilizador obtém a nacionalidade desse país
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Account/GetNationalitiesAsync")]
    [Route("Account/GetNationalitiesAsync")]
    public Task<JsonResult> GetNationalitiesAsync(int countryId)
    {
        var nationalities =
            _countryRepository.GetComboNationalities(countryId);

        return Task.FromResult(
            Json(nationalities.OrderBy(c => c.Text)));

        // return Task.FromResult(Json(nationalities));
    }


    /// <summary>
    /// Aqui o utilizador obtém a lista de países e a respetiva nacionalidade
    /// via JSON para o preenchimento do dropdown-list
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Account/GetCountriesWithNationalitiesAsync")]
    [Route("Account/GetCountriesWithNationalitiesAsync")]
    public Task<JsonResult> GetCountriesWithNationalitiesAsync()
    {
        var countriesWithNationalities =
            _countryRepository.GetComboCountriesAndNationalities();


        // Console.OutputEncoding = Encoding.UTF8;
        // Console.WriteLine(countriesWithNationalities);
        // Console.WriteLine(
        //     Json(countriesWithNationalities
        //         .OrderBy(c => c.Text)));


        return Task.FromResult(
            Json(countriesWithNationalities
                .OrderBy(c => c.Text)));
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // Adicione um método para calcular o tempo restante para o token expirar.
    private TimeSpan GetTimeRemaining(DateTime expirationDate)
    {
        return expirationDate - DateTime.UtcNow;
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
}