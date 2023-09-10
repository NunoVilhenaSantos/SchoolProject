using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.SchoolClasses;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     School class with courses
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class SchoolClassCoursesController : Controller
{
    internal const string SessionVarName = "AllSchoolClassesAndCourses";
    private const string BucketName = "schoolclasscourses";
    private const string SortProperty = "Name";

    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(SchoolClassCourse);
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
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISchoolClassCourseRepository _schoolClassCourseRepository;


    /// <summary>
    ///     School class with courses
    /// </summary>
    /// <param name="context"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="schoolClassCourseRepository"></param>
    public SchoolClassCoursesController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        ISchoolClassCourseRepository schoolClassCourseRepository)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _schoolClassCourseRepository = schoolClassCourseRepository;
    }


    //private List<SchoolClassCourse> GetSchoolClassesAndCourses()
    //{
    //    var schoolClassesWithCourses =
    //        _context.SchoolClassCourses
    //            .Include(s => s.Course)
    //            .Include(s => s.SchoolClass)
    //            .Include(s => s.CreatedBy)
    //            .Include(s => s.UpdatedBy)
    //            .ToList();

    //    return schoolClassesWithCourses;
    //}


    private List<SchoolClassCourse> GetSchoolClassesAndCourses()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var schoolClassesWithCourses = _context.SchoolClassCourses
            .Include(scc => scc.Course)
            .Include(scc => scc.SchoolClass)
            .Include(scc => scc.CreatedBy)
            .Include(scc => scc.UpdatedBy)
            .ToList();


        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Tempo de execução: " +
                          "GetSchoolClassesAndCourses " +
                          $"{elapsedMilliseconds} ms");


        return schoolClassesWithCourses;
    }


    private List<SchoolClassCourse> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<SchoolClassCourse> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<SchoolClassCourse>>(json) ??
                new List<SchoolClassCourse>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetSchoolClassesAndCourses();

            // PaginationViewModel<T>.Initialize(_hostingEnvironment);
            PaginationViewModel<T>
                .Initialize(_hostingEnvironment, _httpContextAccessor);

            // TODO: verificar se assim deixa de dar o erro out of memory
            //
            // Esta 2 classes
            // SchoolClassCoursesController e SchoolClassStudentsController
            //
            // vão usar este método StoreListToFileInJson1
            // para armazenar os dados em ficheiro no formato json
            // PaginationViewModel<SchoolClassCourse>
            //     .StoreListToFileInJson1(recordsQuery, SessionVarName);

            // Armazene a lista na sessão para uso futuro
            // HttpContext.Session.Set(
            //     SessionVarName, Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: SchoolClassCourses
    /// <summary>
    ///     Index, list of school class with courses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<SchoolClassCourse>();
        return View(recordsQuery);
    }


    // GET: SchoolClassCourses
    /// <summary>
    ///     Index with cards, list of school class with courses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<SchoolClassCourse>();
        return View(recordsQuery);
    }


    // GET: SchoolClassCourses
    /// <summary>
    ///     Index with cards, list of school class with courses
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

        var recordsQuery = SessionData<SchoolClassCourse>();

        var model = new PaginationViewModel<SchoolClassCourse>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: SchoolClassCourses/Details/5
    /// <summary>
    ///     Details, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.SchoolClass)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        return schoolClassCourse == null
            ? new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(schoolClassCourse);
    }


    // GET: SchoolClassCourses/Create
    /// <summary>
    ///     Create, school class with courses
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code");

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: SchoolClassCourses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, school class with courses
    /// </summary>
    /// <param name="schoolClassCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SchoolClassCourse schoolClassCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Edit/5
    /// <summary>
    ///     Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);

        if (schoolClassCourse == null)
            return new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schoolClassCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, SchoolClassCourse schoolClassCourse)
    {
        if (id != schoolClassCourse.SchoolClassId)
            return new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassCourseExists(schoolClassCourse.SchoolClassId))
                    return new NotFoundViewResult(
                        nameof(SchoolClassCourseNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }


    // GET: SchoolClassCourses/Delete/5
    /// <summary>
    ///     Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.SchoolClass)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        return schoolClassCourse == null
            ? new NotFoundViewResult(nameof(SchoolClassCourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(schoolClassCourse);
    }


    // POST: SchoolClassCourses/Delete/5
    /// <summary>
    ///     Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);

        if (schoolClassCourse != null)
            _context.SchoolClassCourses.Remove(schoolClassCourse);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    /// <summary>
    /// SchoolClassCourseNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult SchoolClassCourseNotFound => View();


    private bool SchoolClassCourseExists(int id) =>
        _context.SchoolClassCourses
            .Any(e => e.SchoolClassId == id);
}