using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

public class HomeController : Controller
{
    private readonly IStringLocalizer<HomeController> _stringLocalizer;
    private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HomeController> _logger;


    private readonly SignInManager<User> _signInManager;


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


    [ResponseCache(
        Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }


    public IActionResult Index()
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


    public IActionResult About()
    {
        return View();
    }


    public IActionResult Contacts()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult CookiesPrivacy()
    {
        return View();
    }


    public IActionResult Cookies()
    {
        return View();
    }


    public IActionResult Legal()
    {
        return View();
    }


    public IActionResult TimeTracker()
    {
        return View();
    }


    public IActionResult Threads()
    {
        return View();
    }

    public IActionResult Features()
    {
        return View();
    }

    public IActionResult Pricing()
    {
        return View();
    }

    public IActionResult FaQs()
    {
        return View();
    }

    public IActionResult Team()
    {
        return View();
    }

    public IActionResult Locations()
    {
        return View();
    }

    public IActionResult Terms()
    {
        return View();
    }
}