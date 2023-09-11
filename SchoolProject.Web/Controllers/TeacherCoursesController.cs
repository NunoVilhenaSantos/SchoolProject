using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     TeacherCoursesController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class TeacherCoursesController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(TeacherCourse);
    private const string CurrentAction = nameof(Index);

    internal string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "FirstName";


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
    private readonly ITeacherCourseRepository _teacherCourseRepository;


    /// <summary>
    ///     TeacherCoursesController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherCourseRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public TeacherCoursesController(
        DataContextMySql context,
        ITeacherCourseRepository teacherCourseRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _teacherCourseRepository = teacherCourseRepository;
    }


    private List<TeacherCourse> GetTeacherCoursesList()
    {
        var teacherCoursesList =
            _context.TeacherCourses
                // --------------------- Discipline section --------------------- //
                .Include(tc => tc.Course)

                // --------------------- Teacher section -------------------- //
                .Include(tc => tc.Teacher)
                .ThenInclude(t => t.Country)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(c => c.CreatedBy)
                .Include(tc => tc.Teacher)
                .ThenInclude(t => t.CountryOfNationality)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(c => c.CreatedBy)
                .Include(tc => tc.Teacher)
                .ThenInclude(t => t.Birthplace)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(c => c.CreatedBy)
                .Include(tc => tc.Teacher)
                .ThenInclude(t => t.Gender)
                .ThenInclude(g => g.CreatedBy)
                .Include(tc => tc.Teacher)
                .ThenInclude(t => t.User)

                // --------------------- Others section --------------------- //
                .Include(tc => tc.CreatedBy)
                .Include(tc => tc.UpdatedBy)
                .ToList();

        return teacherCoursesList;
    }


    private List<TeacherCourse> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<TeacherCourse> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<TeacherCourse>>(json) ??
                new List<TeacherCourse>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetTeacherCoursesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json =
                PaginationViewModel<TeacherCourse>.StoreListToFileInJson(
                    recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: TeacherCourses
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<TeacherCourse>();
        return View(recordsQuery);
    }


    // GET: TeacherCourses
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<TeacherCourse>();
        return View(recordsQuery);
    }


    // GET: TeacherCourses
    /// <summary>
    ///     IndexCards1 method for the cards view with pagination, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<TeacherCourse>();

        var model = new PaginationViewModel<TeacherCourse>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: TeacherCourses/Details/5
    /// <summary>
    ///     Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(m => m.TeacherId == id);

        return teacherCourse == null
            ? new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(teacherCourse);
    }

    // GET: TeacherCourses/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code");

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        ViewData["TeacherId"] =
            new SelectList(_context.Teachers,
                "Id", "Address");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: TeacherCourses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <param name="teacherCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeacherCourse teacherCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacherCourse);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                teacherCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.CreatedById);

        ViewData["TeacherId"] =
            new SelectList(_context.Teachers,
                "Id", "Address",
                teacherCourse.TeacherId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.UpdatedById);

        return View(teacherCourse);
    }


    // GET: TeacherCourses/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var teacherCourse = await _context.TeacherCourses.FindAsync(id);

        if (teacherCourse == null)
            return new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                teacherCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.CreatedById);

        ViewData["TeacherId"] =
            new SelectList(_context.Teachers,
                "Id", "Address",
                teacherCourse.TeacherId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.UpdatedById);

        return View(teacherCourse);
    }

    // POST: TeacherCourses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="teacherCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TeacherCourse teacherCourse)
    {
        if (id != teacherCourse.TeacherId)
            return new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacherCourse);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(teacherCourse.TeacherId))
                    return new NotFoundViewResult(
                        nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                teacherCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.CreatedById);

        ViewData["TeacherId"] =
            new SelectList(_context.Teachers,
                "Id", "Address",
                teacherCourse.TeacherId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                teacherCourse.UpdatedById);

        return View(teacherCourse);
    }


    // GET: TeacherCourses/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(m => m.TeacherId == id);

        return teacherCourse == null
            ? new NotFoundViewResult(
                nameof(TeacherCourseNotFound), CurrentClass, id.ToString(), CurrentController, nameof(Index))
            : View(teacherCourse);
    }


    // POST: TeacherCourses/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacherCourse = await _context.TeacherCourses.FindAsync(id);

        if (teacherCourse != null)
            _context.TeacherCourses.Remove(teacherCourse);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    /// <summary>
    /// TeacherCourseNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TeacherCourseNotFound => View();


    private bool TeacherCourseExists(int id) =>
        _context.TeacherCourses.Any(e => e.TeacherId == id);
}