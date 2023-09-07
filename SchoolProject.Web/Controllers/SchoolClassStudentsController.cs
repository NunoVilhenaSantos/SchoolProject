using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.SchoolClasses;
using SchoolProject.Web.Models;


namespace SchoolProject.Web.Controllers;

/// <summary>
///     SchoolClassStudentsController
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class SchoolClassStudentsController : Controller
{
    internal const string SessionVarName = "AllSchoolClassesAndStudent";
    private const string BucketName = "schoolclassstudents";
    private const string SortProperty = "Name";


    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ISchoolClassStudentRepository
        _schoolClassStudentRepository;


    /// <summary>
    ///     SchoolClassStudentsController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="schoolClassStudentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public SchoolClassStudentsController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor,
        ISchoolClassStudentRepository schoolClassStudentRepository
    )
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _schoolClassStudentRepository = schoolClassStudentRepository;
    }


    private List<SchoolClassStudent> GetSchoolClassesAndStudent()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var schoolClassesStudentList =
            _context.SchoolClassStudents
                // ------------------ SchoolClass section ------------------- //
                .Include(scs => scs.SchoolClass)

                // -------------------- Student section --------------------- //
                .Include(scs => scs.Student)
                .ThenInclude(s => s.Country)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(n => n.CreatedBy)
                .Include(scs => scs.Student)
                .ThenInclude(s => s.CountryOfNationality)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(n => n.CreatedBy)
                .Include(scs => scs.Student)
                .ThenInclude(s => s.Birthplace)
                .ThenInclude(c => c.Nationality)
                .ThenInclude(n => n.CreatedBy)
                .Include(scs => scs.Student)
                .ThenInclude(s => s.Gender)
                .ThenInclude(g => g.CreatedBy)
                .Include(scs => scs.Student)
                .ThenInclude(s => s.User)

                // ----------------- Student Others section ----------------- //
                .Include(scs => scs.Student)
                .ThenInclude(s => s.SchoolClassStudents)

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


    private List<SchoolClassStudent> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<SchoolClassStudent> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<SchoolClassStudent>>(json) ??
                new List<SchoolClassStudent>();
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
            PaginationViewModel<SchoolClassStudent>
                .StoreListToFileInJson1(recordsQuery, SessionVarName);

            // Armazene a lista na sessão para uso futuro
            // HttpContext.Session.Set(
            //     SessionVarName, Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: SchoolClassStudents
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
        var recordsQuery = SessionData<SchoolClassStudent>();
        return View(recordsQuery);
    }


    // GET: SchoolClassStudents
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
        var recordsQuery = SessionData<SchoolClassStudent>();
        return View(recordsQuery);
    }


    // GET: SchoolClassStudents
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
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<SchoolClassStudent>();

        var model = new PaginationViewModel<SchoolClassStudent>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: SchoolClassStudents/Details/5
    /// <summary>
    ///     Details, details of a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassStudent = await _context.SchoolClassStudents
            .Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassStudent == null) return NotFound();

        return View(schoolClassStudent);
    }


    // GET: SchoolClassStudents/Create
    /// <summary>
    ///     Create, create a new SchoolClassStudent
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym");

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: SchoolClassStudents/Create
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
        SchoolClassStudent schoolClassStudent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassStudent);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }


    // GET: SchoolClassStudents/Edit/5
    /// <summary>
    ///     Edit, edit a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassStudent =
            await _context.SchoolClassStudents.FindAsync(id);

        if (schoolClassStudent == null) return NotFound();

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }

    // POST: SchoolClassStudents/Edit/5
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
        int id, SchoolClassStudent schoolClassStudent)
    {
        if (id != schoolClassStudent.SchoolClassId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassStudent);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassStudentExists(schoolClassStudent.SchoolClassId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }


    // GET: SchoolClassStudents/Delete/5
    /// <summary>
    ///     Delete, delete a SchoolClassStudent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassStudent = await _context.SchoolClassStudents
            .Include(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassStudent == null) return NotFound();

        return View(schoolClassStudent);
    }


    // POST: SchoolClassStudents/Delete/5
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
            await _context.SchoolClassStudents.FindAsync(id);

        if (schoolClassStudent != null)
            _context.SchoolClassStudents.Remove(schoolClassStudent);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool SchoolClassStudentExists(int id)
    {
        return _context.SchoolClassStudents
            .Any(e => e.SchoolClassId == id);
    }
}