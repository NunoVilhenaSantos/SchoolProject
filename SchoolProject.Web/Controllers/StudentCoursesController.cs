using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Controllers;

public class StudentCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public StudentCoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: StudentCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.StudentCourses.Include(s => s.Course)
            .Include(s => s.CreatedBy).Include(s => s.Student)
            .Include(s => s.UpdatedBy);
        return View(await dataContextMySql.ToListAsync());
    }

    // GET: StudentCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.Student)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // GET: StudentCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["StudentId"] =
            new SelectList(_context.Students, "Id", "Address");
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: StudentCourses/Create
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "StudentId,CourseId,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            studentCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.UpdatedById);
        return View(studentCourse);
    }

    // GET: StudentCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            studentCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.UpdatedById);
        return View(studentCourse);
    }

    // POST: StudentCourses/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "StudentId,CourseId,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
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

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            studentCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            studentCourse.UpdatedById);
        return View(studentCourse);
    }

    // GET: StudentCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.Student)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // POST: StudentCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.StudentCourses == null)
            return Problem(
                "Entity set 'DataContextMySql.StudentCourses'  is null.");
        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse != null)
            _context.StudentCourses.Remove(studentCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentCourseExists(int id)
    {
        return (_context.StudentCourses?.Any(e => e.StudentId == id))
            .GetValueOrDefault();
    }
}