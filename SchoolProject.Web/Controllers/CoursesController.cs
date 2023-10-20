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
///     SchoolClassesController
/// </summary>
//[Authorize(Roles = "Admin,SuperUser")]
public class CoursesController : Controller
{
    // Obtém o tipo da classe atual
    internal const string CurrentClass = nameof(Course);
    internal const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Code";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ICourseRepository _schoolClassRepository;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     SchoolClassesController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="schoolClassRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CoursesController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        ICourseRepository schoolClassRepository
    )
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _schoolClassRepository = schoolClassRepository;
    }


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


    /// <summary>
    ///     SchoolClassNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult SchoolClassNotFound => View();


    private List<Course> GetSchoolClasses()
    {
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

            recordsQuery =
                JsonConvert.DeserializeObject<List<Course>>(json) ??
                new List<Course>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetSchoolClasses();

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
    ///     Index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
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
    ///     Index cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
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


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index cards 1 method, for the main view, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
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
    ///     Details of a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClass = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        return schoolClass == null
            ? new NotFoundViewResult(nameof(SchoolClassNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(schoolClass);
    }

    // GET: Courses/Create
    /// <summary>
    ///     Create a new school class, view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }


    // POST: Courses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create a new school class, post.
    ///     validates and saves the new school class.
    /// </summary>
    /// <param name="schoolClass"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course schoolClass)
    {
        if (!ModelState.IsValid) return View(schoolClass);

        _context.Add(schoolClass);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Edit/5
    /// <summary>
    ///     Edit a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClass = await _context.Courses.FindAsync(id);

        return schoolClass == null
            ? new NotFoundViewResult(nameof(SchoolClassNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(schoolClass);
    }

    // POST: Courses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit a school class, post.
    ///     validate and save the edited school class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schoolClass"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course schoolClass)
    {
        if (id != schoolClass.Id)
            return new NotFoundViewResult(nameof(SchoolClassNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(schoolClass);

        try
        {
            _context.Update(schoolClass);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SchoolClassExists(schoolClass.Id))
                return new NotFoundViewResult(nameof(SchoolClassNotFound),
                    CurrentClass, id.ToString(), CurrentController,
                    nameof(Index));

            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Delete/5
    /// <summary>
    ///     Delete a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClass = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (schoolClass == null)
            return new NotFoundViewResult(nameof(SchoolClassNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        return View(schoolClass);
    }


    // POST: Courses/Delete/5
    /// <summary>
    ///     Delete a school class, post.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClass = await _context.Courses.FindAsync(id);

        if (schoolClass != null) _context.Courses.Remove(schoolClass);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool SchoolClassExists(int id)
    {
        return _context.Courses.Any(e => e.Id == id);
    }
}