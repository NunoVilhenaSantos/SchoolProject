using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     courses controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesController : Controller
{
    internal const string SessionVarName = "AllCoursesList";
    private const string BucketName = "courses";
    private const string SortProperty = "Code";

    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Course);
    private const string CurrentAction = nameof(Index);

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




    private readonly DataContextMySql _context;
    private readonly ICourseRepository _courseRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;


    /// <summary>
    ///     constructor for the courses controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="courseRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CoursesController(
        DataContextMySql context,
        ICourseRepository courseRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _courseRepository = courseRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    private List<Course> GetCoursesList()
    {
        //var coursesList =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        return _context.Courses.ToList();
    }


    private List<Course> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Course> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Course>>(json) ??
                           new List<Course>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetCoursesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Course>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Course>();
        return View(recordsQuery);
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Course>();
        return View(recordsQuery);
    }


    // GET: Courses
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<Course>();

        var model = new PaginationViewModel<Course>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Courses/Details/5
    /// <summary>
    ///     Details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        return course == null
            ? new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(course);
    }


    // GET: Courses/Create
    /// <summary>
    ///     Create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create() => View();


    // POST: Courses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create action
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid) return View(course);

        _context.Add(course);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Edit/5
    /// <summary>
    ///     Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var course = await _context.Courses.FindAsync(id);

        return course == null
            ? new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(course);
    }

    // POST: Courses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(course);

        try
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(course.Id))
                return new NotFoundViewResult(
                    nameof(CourseNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));
            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Delete/5
    // /// <summary>
    // ///     Delete action
    // /// </summary>
    // /// <param name="id"></param>
    // /// <returns></returns>
    // public async Task<IActionResult> Delete(int? id)
    // {
    //     if (id == null)  return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));
    //
    //     var course = await _context.Courses
    //         .FirstOrDefaultAsync(m => m.Id == id);
    //
    //     if (course == null)  return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));
    //
    //     return View(course);
    // }


    // POST: Courses/Delete/5
    // /// <summary>
    // ///     Delete action
    // /// </summary>
    // /// <param name="id"></param>
    // /// <returns></returns>
    // [HttpPost]
    // [ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     var course = await _context.Courses.FindAsync(id);
    //
    //     if (course != null) _context.Courses.Remove(course);
    //
    //     await _context.SaveChangesAsync();
    //
    //     return RedirectToAction(nameof(Index));
    // }


    // GET: Countries/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // var country = await _countryRepository.GetByIdAsync(id.Value);

        // if (country == null)  return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        // await _countryRepository.DeleteAsync(country);

        // return RedirectToAction(nameof(Index));

        var course = await _context.Courses.FindAsync(id.Value);

        if (course == null)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        _context.Courses.Remove(course);

        // await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    /// <summary>
    /// CourseNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CourseNotFound => View();


    private bool CourseExists(int id) =>
        _context.Courses.Any(e => e.Id == id);
}