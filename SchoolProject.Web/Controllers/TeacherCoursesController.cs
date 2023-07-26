using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Controllers;

public class TeacherCoursesController : Controller
{
    private readonly DataContextMsSql _context;

    public TeacherCoursesController(DataContextMsSql context)
    {
        _context = context;
    }

    // GET: TeacherCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMsSql = _context.TeacherCourses.Include(t => t.Course)
            .Include(t => t.Teacher);
        return View(await dataContextMsSql.ToListAsync());
    }

    // GET: TeacherCourses/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (teacherCourse == null) return NotFound();

        return View(teacherCourse);
    }

    // GET: TeacherCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        ViewData["TeacherId"] =
            new SelectList(_context.Teachers, "Id", "Address");
        return View();
    }

    // POST: TeacherCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("TeacherId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        TeacherCourse teacherCourse)
    {
        if (ModelState.IsValid)
        {
            teacherCourse.IdGuid = Guid.NewGuid();
            _context.Add(teacherCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            teacherCourse.CourseId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        return View(teacherCourse);
    }

    // GET: TeacherCourses/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            teacherCourse.CourseId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        return View(teacherCourse);
    }

    // POST: TeacherCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("TeacherId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        TeacherCourse teacherCourse)
    {
        if (id != teacherCourse.IdGuid) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacherCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(teacherCourse.IdGuid))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name",
            teacherCourse.CourseId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        return View(teacherCourse);
    }

    // GET: TeacherCourses/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (teacherCourse == null) return NotFound();

        return View(teacherCourse);
    }

    // POST: TeacherCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.TeacherCourses == null)
            return Problem(
                "Entity set 'DataContextMsSql.TeacherCourses'  is null.");
        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse != null)
            _context.TeacherCourses.Remove(teacherCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherCourseExists(Guid id)
    {
        return (_context.TeacherCourses?.Any(e => e.IdGuid == id))
            .GetValueOrDefault();
    }
}