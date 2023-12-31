﻿using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Errors;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     HomeController class.
/// </summary>
public class HomeController : Controller
{
    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;
    // private readonly IConnectivityChecker _connectivityChecker;

    // host environment
    private readonly IWebHostEnvironment _hostingEnvironment;

    // html localizer
    private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;

    // http context accessor
    private readonly IHttpContextAccessor _httpContextAccessor;


    // logger
    private readonly ILogger<HomeController> _logger;


    // session variable names and types
    private readonly Dictionary<string, Type> _sessionVariableTypes =
        new()
        {
            // ---------- A ---------- //
            // {AccountController.SessionVarName, typeof(AppUser)},
            // {AppUsersController.SessionVarName, typeof(AppUser)},
            // ---------- C ---------- //
            {CitiesController.SessionVarName, typeof(City)},
            {CountriesController.SessionVarName, typeof(Country)},
            {CoursesController.SessionVarName, typeof(Course)},
            {
                CoursesDisciplinesController.SessionVarName,
                typeof(CourseDiscipline)
            },
            {CoursesStudentsController.SessionVarName, typeof(CourseStudent)},
            // ---------- D ---------- //
            {DisciplinesController.SessionVarName, typeof(Discipline)},
            // ---------- E ---------- //
            {EnrollmentsController.SessionVarName, typeof(Enrollment)},
            // ---------- G ---------- //
            {GendersController.SessionVarName, typeof(Gender)},
            // ---------- N ---------- //
            {NationalitiesController.SessionVarName, typeof(Nationality)},
            // ---------- R ---------- //
            {RolesController.SessionVarName, typeof(IdentityRole)},
            // ---------- S ---------- //
            {
                StudentDisciplinesController.SessionVarName,
                typeof(StudentDiscipline)
            },
            {StudentsController.SessionVarName, typeof(Student)},
            // ---------- T ---------- //
            {
                TeacherDisciplinesController.SessionVarName,
                typeof(TeacherDiscipline)
            },
            {TeachersController.SessionVarName, typeof(Teacher)},
            // ---------- U ---------- //
            {UsersController.SessionVarName, typeof(UserWithRolesViewModel)}
            // Adicione mais nomes de variáveis de sessão aqui com os tipos correspondentes
        };

    // sign in manager
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IStringLocalizer<HomeController> _stringLocalizer;


    /// <summary>
    ///     HomeController constructor.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="signInManager"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="htmlLocalizer"></param>
    /// <param name="stringLocalizer"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    public HomeController(
        ILogger<HomeController> logger,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        IHtmlLocalizer<HomeController> htmlLocalizer,
        IStringLocalizer<HomeController> stringLocalizer,
        AuthenticatedUserInApp authenticatedUserInApp)
    {
        _logger = logger;
        _htmlLocalizer = htmlLocalizer;
        _signInManager = signInManager;
        _stringLocalizer = stringLocalizer;
        _authenticatedUserInApp = authenticatedUserInApp;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
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
            {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }


    /// <summary>
    ///     Index action.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
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


    private int GetSessionDataSize(string sessionVarName)
    {
        return HttpContext.Session
            .TryGetValue(sessionVarName, out var allData)
            ? allData.Length
            : 0;
    }


    private int GetSessionDataObjectsCount<T>(string sessionVarName)
    {
        // Se os dados não estiverem na sessão, retorne 0.
        if (!HttpContext.Session.TryGetValue(sessionVarName, out var allData))
            return 0;

        // Isso depende de como os dados estão estruturados na sessão.
        var json = Encoding.UTF8.GetString(allData);

        var objectsList = JsonConvert.DeserializeObject<List<T>>(json);

        // Contagem de objetos e retorne essa contagem de objetos.
        if (objectsList == null) return 0;

        var objectsCount = objectsList.Count;
        return objectsCount;
    }


    private List<VariableInfoViewModel> GetVariableInfoList()
    {
        return (from sessionVarName in _sessionVariableTypes.Keys
                let sessionVarType = _sessionVariableTypes[sessionVarName]
                let dataSize = GetSessionDataSize(sessionVarName)
                let objectsCount =
                    GetSessionDataObjectsCount<object>(sessionVarName)
                select new VariableInfoViewModel
                {
                    VariableName = sessionVarName,
                    ClassName = sessionVarType.Name,
                    ObjectsCount = objectsCount,
                    SizeInBytes = dataSize
                })
            .ToList();
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult VariableInfo()
    {
        var viewModel = GetVariableInfoList();

        return View(viewModel);
    }


    /// <summary>
    ///     About action.
    /// </summary>
    /// <returns></returns>
    public IActionResult About()
    {
        return View();
    }


    /// <summary>
    ///     Contact action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Contacts()
    {
        return View();
    }


    /// <summary>
    ///     CookiesPrivacy action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Privacy()
    {
        return View();
    }


    /// <summary>
    ///     CookiesPrivacy action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CookiesPrivacy()
    {
        return View();
    }


    /// <summary>
    ///     Cookies action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Cookies()
    {
        return View();
    }


    /// <summary>
    ///     Legal action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Legal()
    {
        return View();
    }


    /// <summary>
    ///     TimeTracker action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TimeTracker()
    {
        return View();
    }


    /// <summary>
    ///     Threads action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Threads()
    {
        return View();
    }


    /// <summary>
    ///     Features action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Features()
    {
        return View();
    }


    /// <summary>
    ///     Pricing action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Pricing()
    {
        return View();
    }


    /// <summary>
    ///     FaQs action.
    /// </summary>
    /// <returns></returns>
    public IActionResult FaQs()
    {
        return View();
    }


    /// <summary>
    ///     Locations action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Team()
    {
        return View();
    }


    /// <summary>
    ///     Locations action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Locations()
    {
        return View();
    }


    /// <summary>
    ///     Terms action.
    /// </summary>
    /// <returns></returns>
    public IActionResult Terms()
    {
        return View();
    }


    // -------------------------------------------------------------- //


    /// <summary>
    ///     Splits a camelCase string into a list of words, separated by spaces.
    /// </summary>
    /// <param name="text">The camelCase string to split.</param>
    /// <returns>A list of words, separated by spaces.</returns>
    public static string SplitCamelCase(string text)
    {
        // Argument validation
        if (text is null)
            throw new ArgumentNullException(nameof(text));


        // Split the string at every uppercase letter, except for the first letter.
        var textSplit =
            Regex.Replace(text, @"([A-Z][a-z]+)", @" $1");


        // Remove the "Controller" suffix, if it exists.
        if (textSplit.EndsWith("Controller"))
            textSplit = textSplit
                .Substring(0, textSplit.Length - "Controller".Length);


        // Remove the "Controller" suffix
        return textSplit.Replace("Controller", "");
    }


    // -------------------------------------------------------------- //
}