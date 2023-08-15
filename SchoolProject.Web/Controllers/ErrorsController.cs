using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

public class ErrorsController : Controller
{
    private readonly ILogger<ErrorsController> _logger;


    public ErrorsController(ILogger<ErrorsController> logger)
    {
        _logger = logger;
    }


    [ResponseCache(
        Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }


    [Route("error/403")]
    public IActionResult Error403()
    {
        return View();
    }


    [Route("error/404")]
    public IActionResult Error404()
    {
        return View();
    }


    [Route("error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        var viewModel = new ErrorViewModel
        {
            StatusCode = statusCode,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        return View("Error", viewModel);
    }
}