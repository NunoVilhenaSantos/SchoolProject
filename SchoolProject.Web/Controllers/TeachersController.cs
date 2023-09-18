using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     TeachersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class TeachersController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Teacher);
    private const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "FirstName";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ITeacherRepository _teacherRepository;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     TeachersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public TeachersController(
        DataContextMySql context,
        ITeacherRepository teacherRepository,
        IWebHostEnvironment hostingEnvironment
    )
    {
        _context = context;
        _teacherRepository = teacherRepository;
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
    ///     TeacherNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TeacherNotFound => View();


    private List<Teacher> GetTeachersList()
    {
        return _context.Teachers
            .Include(t => t.Country)
            .ThenInclude(c => c.Nationality)
            .Include(t => t.Country)
            .ThenInclude(c => c.CreatedBy)
            .Include(t => t.City)
            .ThenInclude(c => c.CreatedBy)
            .Include(t => t.CountryOfNationality)
            .ThenInclude(c => c.Nationality)
            .Include(t => t.CountryOfNationality)
            .ThenInclude(c => c.CreatedBy)
            .Include(t => t.Birthplace)
            .ThenInclude(c => c.Nationality)
            .Include(t => t.Birthplace)
            .ThenInclude(c => c.CreatedBy)
            .Include(t => t.Gender)
            .ThenInclude(g => g.CreatedBy)
            .Include(t => t.User)
            // Se desejar carregar os cursos associados
            .Include(t => t.TeacherCourses)
            // E seus detalhes, se necessário
            .ThenInclude(tc => tc.Course).ToList();
    }


    private List<Teacher> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Teacher> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Teacher>>(json) ??
                           new List<Teacher>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetTeachersList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Teacher>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Teachers
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

        var recordsQuery = SessionData<Teacher>();
        return View(recordsQuery);
    }


    // GET: Teachers
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

        var recordsQuery = SessionData<Teacher>();
        return View(recordsQuery);
    }


    // GET: Teachers
    /// <summary>
    ///     IndexCards1 method for the cards view, for testing purposes.
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

        var recordsQuery = SessionData<Teacher>();

        var model = new PaginationViewModel<Teacher>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Teachers/Details/5
    /// <summary>
    ///     Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }


    // GET: Teachers/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Teachers/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for adding a new teacher.
    /// </summary>
    /// <param name="teacher"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher)
    {
        if (!ModelState.IsValid) return View(teacher);

        _context.Add(teacher);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Teachers/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _context.Teachers.FindAsync(id);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }


    // POST: Teachers/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for editing a teacher.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="teacher"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Teacher teacher)
    {
        if (id != teacher.Id)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(teacher);

        try
        {
            _context.Update(teacher);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherExists(teacher.Id))
                return new NotFoundViewResult(
                    nameof(TeacherNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Teachers/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }

    // POST: Teachers/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for deleting a teacher.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher != null) _context.Teachers.Remove(teacher);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool TeacherExists(int id)
    {
        return _context.Teachers.Any(e => e.Id == id);
    }
}