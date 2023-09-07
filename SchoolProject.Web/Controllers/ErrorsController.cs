﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     ErrorsController class.
/// </summary>
public class ErrorsController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;

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
}