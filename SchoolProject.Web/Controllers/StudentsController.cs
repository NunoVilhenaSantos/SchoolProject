using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     students controller
/// </summary>
public class StudentsController : Controller
{
    private const string BucketName = "students";
    private readonly DataContextMySql _context;
    private readonly IStudentRepository _studentRepository;


    /// <summary>
    ///     students controller constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentRepository"></param>
    public StudentsController(
        DataContextMySql context, IStudentRepository studentRepository
    )
    {
        _context = context;
        _studentRepository = studentRepository;
    }


    // GET: Students
    /// <summary>
    ///     students index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetStudentsList());
    }


    private List<Student> GetStudentsList() => _context.Students.ToList();


    // GET: Students
    /// <summary>
    ///     students index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetStudentsList());
    }


    // GET: Students
    // /// <summary>
    // ///  students index, for the main view, for testing purposes.
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // [HttpGet]
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = GetStudentsListForCards(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Student>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


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
        string sortOrder = "asc", string sortProperty = "FirstName")
    {
        // TODO: Fix the sort order
        var model = new PaginationViewModel<Student>(
            GetStudentsList(),
            pageNumber, pageSize,
            _context.Students.Count(),
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
        if (id == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student == null) return NotFound();

        return View(student);
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
        if (id == null) return NotFound();

        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: Students/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        if (id != student.Id) return NotFound();

        if (!ModelState.IsValid) return View(student);

        try
        {
            _context.Update(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(student.Id)) return NotFound();

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
        if (id == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student == null) return NotFound();

        return View(student);
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