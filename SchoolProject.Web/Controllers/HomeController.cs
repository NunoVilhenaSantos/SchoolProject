using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HomeController> _logger;


    public HomeController(
        ILogger<HomeController> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }


    public IActionResult Index()
    {
        // Verificar a conectividade de rede
        //if (_httpContextAccessor.HttpContext != null)
        //{
        //    var connectivityChecker =
        //        _httpContextAccessor.HttpContext
        //            .RequestServices.GetRequiredService<IConnectivityChecker>();

        //    var isConnected = connectivityChecker.ConnectivityCheckingEnabled;
        //    var conneted = connectivityChecker.ForceCheck();

        //    if (conneted.IsFailed)
        //    {

        //    }

        //    // Registrar no log
        //    _logger.LogInformation(
        //        $"Conectividade de rede: " +
        //        $"{(isConnected ? "Conectado" : "Desconectado")}");
        //}
        //else
        //{
        //}

        // Resto da lógica do controlador
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

}