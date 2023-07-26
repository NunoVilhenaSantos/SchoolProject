using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Controllers;

public class StudentCoursesController : Controller
{
    private readonly DataContextMsSql _context;

    public StudentCoursesController(DataContextMsSql context)
    {
        _context = context;
    }

    // GET: StudentCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMsSql = _context.StudentCourses.Include(s => s.Course)
            .Include(s => s.Student);
        return View(await dataContextMsSql.ToListAsync());
    }

    // GET: StudentCourses/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // GET: StudentCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        ViewData["StudentId"] =
            new SelectList(_context.Students, "Id", "Address");
        return View();
    }

    // POST: StudentCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("StudentId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
    {
        if (ModelState.IsValid)
        {
            studentCourse.IdGuid = Guid.NewGuid();
            _context.Add(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            studentCourse.CourseId);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        return View(studentCourse);
    }

    // GET: StudentCourses/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            studentCourse.CourseId);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        return View(studentCourse);
    }

    // POST: StudentCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("StudentId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
    {
        if (id != studentCourse.IdGuid) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(studentCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(studentCourse.IdGuid))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            studentCourse.CourseId);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", studentCourse.StudentId);
        return View(studentCourse);
    }

    // GET: StudentCourses/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(s => s.Course)
            .Include(s => s.Student)
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // POST: StudentCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.StudentCourses == null)
            return Problem(
                "Entity set 'DataContextMsSql.StudentCourses'  is null.");
        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse != null)
            _context.StudentCourses.Remove(studentCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentCourseExists(Guid id)
    {
        return (_context.StudentCourses?.Any(e => e.IdGuid == id))
            .GetValueOrDefault();
    }
}