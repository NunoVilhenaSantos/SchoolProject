using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///    StudentCoursesController
/// </summary>
public class StudentCoursesController : Controller
{
    private readonly IStudentCourseRepository _studentCourseRepository;
    private readonly DataContextMySql _context;

    /// <summary>
    ///   StudentCoursesController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentCourseRepository"></param>
    public StudentCoursesController(
        DataContextMySql context,
        IStudentCourseRepository studentCourseRepository
    )
    {
        _context = context;
        _studentCourseRepository = studentCourseRepository;
    }


    // GET: StudentCourses
    /// <summary>
    ///  Index, list all StudentCourses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetStudentCourses());
    }


    private IEnumerable<StudentCourse> GetStudentCourses()
    {
        var studentCoursesList =
            _context.StudentCourses
                .Include(s => s.Course)
                .Include(s => s.Student)
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy);

        return studentCoursesList;
    }


    // GET: StudentCourses
    /// <summary>
    /// Index with cards, list all StudentCourses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetStudentCourses());
    }


    // GET: StudentCourses
    // /// <summary>
    // /// Index, list all StudentCourses
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records =
    //         GetStudentCourses(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<StudentCourse>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<StudentCourse> GetStudentCourses(
        int pageNumber, int pageSize)
    {
        var records = GetStudentCourses()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return records;
    }


    // GET: StudentCourses
    /// <summary>
    /// Index with cards, list all StudentCourses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records =
            GetStudentCourses(pageNumber, pageSize);

        var model = new PaginationViewModel<StudentCourse>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Teachers.Count(),
        };

        return View(model);
    }


    // GET: StudentCourses/Details/5
    /// <summary>
    /// Details, show details of a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // GET: StudentCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] =
            new SelectList(_context.Courses, "Id", "Code");

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
    /// Create, create a new StudentCourse
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

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
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
    /// Edit, edit a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var studentCourse = await _context.StudentCourses.FindAsync(id);

        if (studentCourse == null) return NotFound();

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
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
    /// Edit, edit a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <param name="studentCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentCourse studentCourse)
    {
        if (id != studentCourse.StudentId) return NotFound();

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
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
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
    /// Delete, delete a StudentCourse
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }


    // POST: StudentCourses/Delete/5
    /// <summary>
    /// Delete, delete a StudentCourse
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

    private bool StudentCourseExists(int id) =>
        _context.StudentCourses.Any(e => e.StudentId == id);
}