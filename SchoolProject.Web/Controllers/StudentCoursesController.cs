using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MSSQL;
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
        return _context.StudentCourses != null
            ? View(await _context.StudentCourses.ToListAsync())
            : Problem(
                "Entity set 'DataContextMsSql.StudentCourses'  is null.");
    }

    // GET: StudentCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // GET: StudentCourses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: StudentCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(studentCourse);
    }

    // GET: StudentCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse == null) return NotFound();

        return View(studentCourse);
    }

    // POST: StudentCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        StudentCourse studentCourse)
    {
        if (id != studentCourse.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(studentCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(studentCourse.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(studentCourse);
    }

    // GET: StudentCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(m => m.Id == id);
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
                "Entity set 'DataContextMsSql.StudentCourses'  is null.");

        var studentCourse = await _context.StudentCourses.FindAsync(id);
        if (studentCourse != null)
            _context.StudentCourses.Remove(studentCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentCourseExists(int id)
    {
        return (_context.StudentCourses?.Any(e => e.Id == id))
            .GetValueOrDefault();
    }
}