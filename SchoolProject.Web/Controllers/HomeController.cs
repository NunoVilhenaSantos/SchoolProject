using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///    HomeController class.
/// </summary>
public class HomeController : Controller
{
    private readonly IStringLocalizer<HomeController> _stringLocalizer;
    private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<HomeController> _logger;


    private readonly SignInManager<User> _signInManager;


    /// <summary>
    ///    HomeController constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="signInManager"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="htmlLocalizer"></param>
    /// <param name="stringLocalizer"></param>
    public HomeController(
        ILogger<HomeController> logger,
        SignInManager<User> signInManager,
        IHttpContextAccessor httpContextAccessor,
        IHtmlLocalizer<HomeController> htmlLocalizer,
        IStringLocalizer<HomeController> stringLocalizer
    )
    {
        _logger = logger;
        _htmlLocalizer = htmlLocalizer;
        _signInManager = signInManager;
        _stringLocalizer = stringLocalizer;
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    ///   Error action.
    /// </summary>
    /// <returns></returns>
    [ResponseCache(
        Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }


    /// <summary>
    ///   Index action.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        // Verificar a conectividade de rede
        // if (_httpContextAccessor.HttpContext != null)
        // {
        //     var connectivityChecker =
        //         _httpContextAccessor.HttpContext
        //             .RequestServices.GetRequiredService<IConnectivityChecker>();
        //
        //     var isConnected = connectivityChecker.ConnectivityCheckingEnabled;
        //     var connected = connectivityChecker.ForceCheck();
        //
        //     if (connected.IsFailed)
        //     {
        //     }
        //
        //     // Registrar no log
        //     _logger.LogInformation(
        //         "Conectividade de rede: {Desconectado}",
        //         (isConnected ? "Conectado" : "Desconectado")
        //     );
        // }
        // else
        // {
        // }

        ViewData["stringLocalizer"] = _stringLocalizer["About Title"];
        ViewData["htmlLocalizer"] =
            _htmlLocalizer["<b>Hello</b><i> {0}</i>",
                _signInManager.IsSignedIn(User)];

        ViewData["WelcomeMessage"] = _stringLocalizer["WelcomeMessage"];
        return View();
    }


    /// <summary>
    ///  About action.
    /// </summary>
    /// <returns></returns>
    public IActionResult About() => View();


    /// <summary>
    /// Contact action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Contacts() => View();


    /// <summary>
    /// CookiesPrivacy action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Privacy() => View();


    /// <summary>
    /// CookiesPrivacy action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CookiesPrivacy() => View();


    /// <summary>
    /// Cookies action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Cookies() => View();


    /// <summary>
    /// Legal action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Legal() => View();


    /// <summary>
    /// TimeTracker action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TimeTracker() => View();


    /// <summary>
    /// Threads action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Threads() => View();


    /// <summary>
    /// Features action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Features() => View();


    /// <summary>
    /// Pricing action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Pricing() => View();


    /// <summary>
    /// FaQs action.
    /// </summary>
    /// <returns></returns>
    public IActionResult FaQs() => View();


    /// <summary>
    /// Locations action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Team() => View();


    /// <summary>
    ///     Locations action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Locations() => View();


    /// <summary>
    /// Terms action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Terms() => View();
}