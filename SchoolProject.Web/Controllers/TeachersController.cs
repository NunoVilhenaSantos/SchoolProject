using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Models;


namespace SchoolProject.Web.Controllers;

/// <summary>
///   TeachersController class.
/// </summary>
public class TeachersController : Controller
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly DataContextMySql _context;

    private const string BucketName = "teachers";


    /// <summary>
    ///  TeachersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherRepository"></param>
    public TeachersController(
        DataContextMySql context,
        ITeacherRepository teacherRepository
    )
    {
        _context = context;
        _teacherRepository = teacherRepository;
    }


    // GET: Teachers
    /// <summary>
    ///  Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    private IEnumerable<Teacher> GetTeachersList() =>
        _context.Teachers.ToListAsync().Result;


    // GET: Teachers
    /// <summary>
    /// IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    // GET: Teachers
    // /// <summary>
    // /// Index1 method, for the main view, for testing purposes.
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // [HttpGet]
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = GetTeachersListForCards(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Teacher>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<Teacher> GetTeachersListForCards(
        int pageNumber, int pageSize) =>
        GetTeachersList()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();


    // GET: Teachers
    /// <summary>
    /// IndexCards1 method for the cards view, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records = GetTeachersListForCards(pageNumber, pageSize);

        var model = new PaginationViewModel<Teacher>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Teachers.Count(),
        };

        return View(model);
    }


    // GET: Teachers/Details/5
    /// <summary>
    /// Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }


    // GET: Teachers/Create
    /// <summary>
    /// Create method, for the create view.
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
    /// Create method, for adding a new teacher.
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
    /// Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }


    // POST: Teachers/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit method, for editing a teacher.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="teacher"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();

        if (!ModelState.IsValid) return View(teacher);

        try
        {
            _context.Update(teacher);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherExists(teacher.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Teachers/Delete/5
    /// <summary>
    /// Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teachers/Delete/5
    /// <summary>
    /// DeleteConfirmed method, for deleting a teacher.
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

    private bool TeacherExists(int id) =>
        _context.Teachers.Any(e => e.Id == id);
}