using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models.Errors;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     HomeController class.
/// </summary>
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<HomeController> _logger;


    private readonly Dictionary<string, Type> _sessionVariableTypes =
        new()
        {
            {CitiesController.SessionVarName, typeof(City)},
            {CountriesController.SessionVarName, typeof(Country)},
            {DisciplinesController.SessionVarName, typeof(Discipline)},
            {EnrollmentsController.SessionVarName, typeof(Enrollment)},
            {GendersController.SessionVarName, typeof(Gender)},
            {NationalitiesController.SessionVarName, typeof(Nationality)},
            {RolesController.SessionVarName, typeof(IdentityRole)},
            {
                CoursesDisciplinesController.SessionVarName,
                typeof(CourseDisciplines)
            },
            {CoursesController.SessionVarName, typeof(Course)},
            {
                CoursesStudentsController.SessionVarName,
                typeof(CourseStudents)
            },
            {StudentCoursesController.SessionVarName, typeof(StudentCourse)},
            {StudentsController.SessionVarName, typeof(Student)},
            {TeacherCoursesController.SessionVarName, typeof(TeacherCourse)},
            {TeachersController.SessionVarName, typeof(Teacher)},
            {UsersController.SessionVarName, typeof(UserWithRolesViewModel)}
            // Adicione mais nomes de variáveis de sessão aqui com os tipos correspondentes
        };


    private readonly SignInManager<User> _signInManager;
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
    public HomeController(
        ILogger<HomeController> logger,
        SignInManager<User> signInManager,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        IHtmlLocalizer<HomeController> htmlLocalizer,
        IStringLocalizer<HomeController> stringLocalizer
    )
    {
        _logger = logger;
        _htmlLocalizer = htmlLocalizer;
        _signInManager = signInManager;
        _stringLocalizer = stringLocalizer;
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
        return HttpContext.Session.TryGetValue(sessionVarName, out var allData)
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
}