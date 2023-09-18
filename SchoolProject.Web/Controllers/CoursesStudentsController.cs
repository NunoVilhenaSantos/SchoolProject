using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     SchoolClassStudentsController
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesStudentsController : Controller
{
    // Obtém o tipo da classe atual
    internal const string CurrentClass = nameof(CourseStudents);
    internal const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Name";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ICourseStudentsRepository
        _schoolClassStudentRepository;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     SchoolClassStudentsController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="schoolClassStudentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CoursesStudentsController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        ICourseStudentsRepository schoolClassStudentRepository
    )
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _schoolClassStudentRepository = schoolClassStudentRepository;
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
    ///     SchoolClassStudentNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult SchoolClassStudentNotFound => View();


    private List<CourseStudents> GetSchoolClassesAndStudent()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var schoolClassesStudentList =
            _context.CoursesStudents
                // ------------------ Discipline section ------------------- //
                .Include(scs => scs.Student)

                // -------------------- Student section --------------------- //
                .Include(scs => scs.Student)
                //.ThenInclude(s => s.Country)
                //.ThenInclude(c => c.Nationality)
                //.ThenInclude(n => n.CreatedBy)
                //.Include(scs => scs.Student)
                //.ThenInclude(s => s.CountryOfNationality)
                //.ThenInclude(c => c.Nationality)
                //.ThenInclude(n => n.CreatedBy)
                //.Include(scs => scs.Student)
                //.ThenInclude(s => s.Birthplace)
                //.ThenInclude(c => c.Nationality)
                //.ThenInclude(n => n.CreatedBy)
                //.Include(scs => scs.Student)
                //.ThenInclude(s => s.Gender)
                //.ThenInclude(g => g.CreatedBy)
                //.Include(scs => scs.Student)
                //.ThenInclude(s => s.User)

                // ----------------- Student Others section ----------------- //
                //.Include(scs => scs.Student)
                //.ThenInclude(s => s.SchoolClassStudents)

                // --------------------- Others section --------------------- //
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy)
                .ToList();

        stopwatch.Stop();
        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine("Tempo de execução: " +
                          "GetSchoolClassesAndCourses " +
                          $"{elapsedMilliseconds} ms");

        return schoolClassesStudentList;
    }


    private List<CourseStudents> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<CourseStudents> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<CourseStudents>>(json) ??
                new List<CourseStudents>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            recordsQuery = GetSchoolClassesAndStudent();

            // PaginationViewModel<T>.Initialize(_hostingEnvironment);
            PaginationViewModel<T>.Initialize(_hostingEnvironment,
                _httpContextAccessor);

            // TODO: verificar se assim deixa de dar o erro out of memory
            //
            // Esta 2 classes
            // SchoolClassCoursesController e SchoolClassStudentsController
            //
            // vão usar este método StoreListToFileInJson1
            // para armazenar os dados em ficheiro no formato json
            // PaginationViewModel<SchoolClassStudent>
            //     .StoreListToFileInJson1(recordsQuery, SessionVarName);

            // Armazene a lista na sessão para uso futuro
            // HttpContext.Session.Set(
            //     SessionVarName, Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: CoursesStudents
    /// <summary>
    ///     Index, list all SchoolClassStudents
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

        var recordsQuery = SessionData<CourseStudents>();
        return View(recordsQuery);
    }


    // GET: CoursesStudents
    /// <summary>
    ///     IndexCards, list all SchoolClassStudents
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

        var recordsQuery = SessionData<CourseStudents>();
        return View(recordsQuery);
    }


    // GET: CoursesStudents
    /// <summary>
    ///     IndexCards1, list all SchoolClassStudents
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

        var recordsQuery = SessionData<CourseStudents>();

        var model = new PaginationViewModel<CourseStudents>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: CoursesStudents/Details/5
    /// <summary>
    ///     Details, details of a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var schoolClassStudent = await _context.CoursesStudents
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        return schoolClassStudent == null
            ? new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(schoolClassStudent);
    }


    // GET: CoursesStudents/Create
    /// <summary>
    ///     Create, create a new SchoolClassStudent
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["DisciplineId"] =
            new SelectList(_context.Courses,
                "Id", "Acronym");

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "FullName");

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName");

        return View();
    }

    // POST: CoursesStudents/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create, create a new SchoolClassStudent
    /// </summary>
    /// <param name="schoolClassStudent"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CourseStudents schoolClassStudent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassStudent);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Courses,
                "Id", "Acronym",
                schoolClassStudent.CourseId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "FullName",
                schoolClassStudent.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.UpdatedBy.Id);

        return View(schoolClassStudent);
    }


    // GET: CoursesStudents/Edit/5
    /// <summary>
    ///     Edit, edit a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var schoolClassStudent =
            await _context.CoursesStudents.FindAsync(id);

        if (schoolClassStudent == null)
            return new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        ViewData["DisciplineId"] =
            new SelectList(_context.Courses,
                "Id", "Acronym",
                schoolClassStudent.CourseId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "FullName",
                schoolClassStudent.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.UpdatedBy.Id);

        return View(schoolClassStudent);
    }

    // POST: CoursesStudents/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit, edit a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schoolClassStudent"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, CourseStudents schoolClassStudent)
    {
        if (id != schoolClassStudent.CourseId)
            return new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassStudent);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassStudentExists(
                        schoolClassStudent.CourseId))
                    return new NotFoundViewResult(
                        nameof(SchoolClassStudentNotFound), CurrentClass,
                        id.ToString(), CurrentController, nameof(Index));
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Courses,
                "Id", "Acronym",
                schoolClassStudent.CourseId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "FullName",
                schoolClassStudent.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                schoolClassStudent.UpdatedBy?.Id);

        return View(schoolClassStudent);
    }


    // GET: CoursesStudents/Delete/5
    /// <summary>
    ///     Delete, delete a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var schoolClassStudent = await _context.CoursesStudents
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.CourseId == id);

        return schoolClassStudent == null
            ? new NotFoundViewResult(
                nameof(SchoolClassStudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(schoolClassStudent);
    }


    // POST: CoursesStudents/Delete/5
    /// <summary>
    ///     DeleteConfirmed, delete a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClassStudent =
            await _context.CoursesStudents.FindAsync(id);

        if (schoolClassStudent != null)
            _context.CoursesStudents.Remove(schoolClassStudent);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool SchoolClassStudentExists(int id)
    {
        return _context.CoursesStudents
            .Any(e => e.CourseId == id);
    }
}