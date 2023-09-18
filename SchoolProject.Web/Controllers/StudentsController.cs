using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     students controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class StudentsController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Student);
    private const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "FirstName";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IStudentRepository _studentRepository;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     students controller constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public StudentsController(
        DataContextMySql context,
        IStudentRepository studentRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _studentRepository = studentRepository;
        _hostingEnvironment = hostingEnvironment;
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
    ///     StudentNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult StudentNotFound => View();


    private List<Student> GetStudentsList()
    {
        return _context.Students
            .Include(s => s.Country)
            .ThenInclude(c => c.Nationality)
            .Include(s => s.Country)
            .ThenInclude(c => c.CreatedBy)
            .Include(s => s.City)
            .ThenInclude(c => c.CreatedBy)
            .Include(s => s.CountryOfNationality)
            .ThenInclude(c => c.Nationality)
            .Include(s => s.CountryOfNationality)
            .ThenInclude(c => c.CreatedBy)
            .Include(s => s.Birthplace)
            .ThenInclude(c => c.Nationality)
            .Include(s => s.Birthplace)
            .ThenInclude(c => c.CreatedBy)
            .Include(s => s.Gender)
            .ThenInclude(g => g.CreatedBy)
            .Include(s => s.User).ToList();

        // Se desejar carregar as turmas associadas
        // .Include(s => s.CoursesStudents)
        // .ThenInclude(scs => scs.Discipline)
        // .ThenInclude(sc => sc.Disciplines)

        // Se desejar carregar os cursos associados
        // E seus detalhes, se necessário
        // .Include(t => t.StudentCourses)
        // .ThenInclude(tc => tc.Discipline)
        // .ToList();
    }


    private List<Student> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Student> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Student>>(json) ??
                           new List<Student>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetStudentsList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Student>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Students
    /// <summary>
    ///     students index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery = SessionData<Student>();
        return View(recordsQuery);
    }


    // GET: Students
    /// <summary>
    ///     students index
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
        var recordsQuery = SessionData<Student>();
        return View(recordsQuery);
    }


    // GET: Students
    /// <summary>
    ///     Index1 method, for the main view, for testing purposes.
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
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<Student>();

        var model = new PaginationViewModel<Student>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Students/Details/5
    /// <summary>
    ///     students details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        return student == null
            ? new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(student);
    }


    // GET: Students/Create
    /// <summary>
    ///     students create
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }


    // POST: Students/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     students create
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        _context.Add(student);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Students/Edit/5
    /// <summary>
    ///     students edit
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _context.Students.FindAsync(id);

        return student == null
            ? new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(student);
    }

    // POST: Students/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     students edit
    /// </summary>
    /// <param name="id"></param>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(student);

        try
        {
            _context.Update(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(student.Id))
                return new NotFoundViewResult(
                    nameof(StudentNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Students/Delete/5
    /// <summary>
    ///     students delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        return student == null
            ? new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(student);
    }


    // POST: Students/Delete/5
    /// <summary>
    ///     students delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student != null) _context.Students.Remove(student);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool StudentExists(int id)
    {
        return _context.Students.Any(e => e.Id == id);
    }
}