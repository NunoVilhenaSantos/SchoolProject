using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     ErrorsController class.
/// </summary>
public class ErrorsController : Controller
{
    // hostingEnvironment is used to check if the application is running in development mode.
    private readonly IWebHostEnvironment _hostingEnvironment;

    // logger is used to log errors.
    private readonly ILogger<ErrorsController> _logger;


    /// <summary>
    ///     ErrorsController constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="hostingEnvironment"></param>
    public ErrorsController(ILogger<ErrorsController> logger,
        IWebHostEnvironment hostingEnvironment)
    {
        _logger = logger;
        _hostingEnvironment = hostingEnvironment;
    }


    /// <summary>
    ///     Error action.
    /// </summary>
    /// <returns></returns>
    [ResponseCache(
        Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var error = exceptionHandlerPathFeature?.Error;

        if (error is DbUpdateException dbUpdateException)
        {
            // Handle DbUpdateException,
            // perhaps by redirecting to a specific error page for database issues.
            _logger.LogError(dbUpdateException, "Database error occurred");

            return RedirectToAction("DatabaseError");
        }

        // Handle other exceptions here,
        // or redirect to a general error page.
        _logger.LogError(error, "An error occurred");

        // return View("Error");

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }


    /// <summary>
    ///     Error403 action.
    /// </summary>
    /// <returns></returns>
    [Route("error/403")]
    public IActionResult Error403()
    {
        return View();
    }


    /// <summary>
    ///     Error404 action.
    /// </summary>
    /// <returns></returns>
    [Route("error/404")]
    public IActionResult Error404()
    {
        return View();
    }


    /// <summary>
    ///     handling error codes.
    /// </summary>
    /// <param name="statusCode"></param>
    /// <returns></returns>
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


    // You can create a specific view for database-related errors if needed.
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Route("error/database-error")]
    public IActionResult DatabaseError(DbErrorViewModel dbErrorViewModel)
    {
        return View("DatabaseError", dbErrorViewModel);
    }
}