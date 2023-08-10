using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public SchoolClassCoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: SchoolClassCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.SchoolClassCourses
            .Include(s => s.Course).Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass).Include(s => s.UpdatedBy);
        return View(await dataContextMySql.ToListAsync());
    }

    // GET: SchoolClassCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);
        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses, "Id", "Acronym");
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: SchoolClassCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "SchoolClassId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        SchoolClassCourse schoolClassCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            schoolClassCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.CreatedById);
        ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id",
            "Acronym", schoolClassCourse.SchoolClassId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.UpdatedById);
        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);
        if (schoolClassCourse == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            schoolClassCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.CreatedById);
        ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id",
            "Acronym", schoolClassCourse.SchoolClassId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.UpdatedById);
        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "SchoolClassId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        SchoolClassCourse schoolClassCourse)
    {
        if (id != schoolClassCourse.SchoolClassId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassCourseExists(schoolClassCourse.SchoolClassId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            schoolClassCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.CreatedById);
        ViewData["SchoolClassId"] = new SelectList(_context.SchoolClasses, "Id",
            "Acronym", schoolClassCourse.SchoolClassId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            schoolClassCourse.UpdatedById);
        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);
        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.SchoolClassCourses == null)
            return Problem(
                "Entity set 'DataContextMySql.SchoolClassCourses'  is null.");
        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);
        if (schoolClassCourse != null)
            _context.SchoolClassCourses.Remove(schoolClassCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SchoolClassCourseExists(int id)
    {
        return (_context.SchoolClassCourses?.Any(e => e.SchoolClassId == id))
            .GetValueOrDefault();
    }
}