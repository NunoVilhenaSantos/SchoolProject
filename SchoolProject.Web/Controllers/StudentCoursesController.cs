using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     StudentCoursesController
/// </summary>
//[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class StudentCoursesController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(StudentCourse);
    private const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Name";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IStudentCourseRepository _studentCourseRepository;

    internal string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    /// <summary>
    ///     StudentCoursesController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentCourseRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public StudentCoursesController(
        DataContextMySql context,
        IStudentCourseRepository studentCourseRepository,
        IWebHostEnvironment hostingEnvironment, AuthenticatedUserInApp authenticatedUserInApp)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _studentCourseRepository = studentCourseRepository;
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
    ///     StudentCourseNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult StudentCourseNotFound => View();


    private List<StudentCourse> GetStudentAndCourses()
    {
        var studentCoursesList =
            _context.StudentCourses
                .Include(s => s.Course)
                .Include(s => s.Student)
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy)
                .ToList();

        return studentCoursesList;
    }


    private List<StudentCourse> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<StudentCourse> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<StudentCourse>>(json) ??
                new List<StudentCourse>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetStudentAndCourses();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<StudentCourse>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: StudentCourses
    /// <summary>
    ///     Index, list all StudentCourses
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

        var recordsQuery = SessionData<StudentCourse>();
        return View(recordsQuery);
    }


    // GET: StudentCourses
    /// <summary>
    ///     Index with cards, list all StudentCourses
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

        var recordsQuery = SessionData<StudentCourse>();
        return View(recordsQuery);
    }


    // GET: StudentCourses
    /// <summary>
    ///     Index with cards, list all StudentCourses
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

        var recordsQuery = SessionData<StudentCourse>();

        var model = new PaginationViewModel<StudentCourse>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: StudentCourses/Details/5
    /// <summary>
    ///     Details, show details of a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return studentCourse == null
            ? new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(studentCourse);
    }

    // GET: StudentCourses/Create
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines, "Id", "Code");

        ViewData["CreatedById"] =
            new SelectList(_context.Users, "Id", "Id");

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users, "Id", "Id");

        return View();
    }

    // POST: StudentCourses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, create a new StudentCourse
    /// </summary>
    /// <param name="studentCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCourse studentCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                studentCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                studentCourse.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.UpdatedById);

        return View(studentCourse);
    }


    // GET: StudentCourses/Edit/5
    /// <summary>
    ///     Edit, edit a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var studentCourse = await _context.StudentCourses.FindAsync(id);

        if (studentCourse == null)
            return new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                studentCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                studentCourse.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.UpdatedById);

        return View(studentCourse);
    }

    // POST: StudentCourses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, edit a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <param name="studentCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentCourse studentCourse)
    {
        if (id != studentCourse.StudentId)
            return new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(studentCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(studentCourse.StudentId))
                    return new NotFoundViewResult(
                        nameof(StudentCourseNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                studentCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                studentCourse.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                studentCourse.UpdatedById);

        return View(studentCourse);
    }


    // GET: StudentCourses/Delete/5
    /// <summary>
    ///     Delete, delete a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return studentCourse == null
            ? new NotFoundViewResult(
                nameof(StudentCourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(studentCourse);
    }


    // POST: StudentCourses/Delete/5
    /// <summary>
    ///     Delete, delete a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var studentCourse = await _context.StudentCourses.FindAsync(id);

        if (studentCourse != null)
            _context.StudentCourses.Remove(studentCourse);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool StudentCourseExists(int id)
    {
        return _context.StudentCourses.Any(e => e.StudentId == id);
    }
}