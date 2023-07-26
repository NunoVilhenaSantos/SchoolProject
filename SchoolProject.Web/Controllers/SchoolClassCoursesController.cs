using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassCoursesController : Controller
{
    private readonly DataContextMsSql _context;

    public SchoolClassCoursesController(DataContextMsSql context)
    {
        _context = context;
    }

    // GET: SchoolClassCourses
    public async Task<IActionResult> Index()
    {
        return _context.SchoolClassCourses != null
            ? View(await _context.SchoolClassCourses.ToListAsync())
            : Problem(
                "Entity set 'DataContextMsSql.SchoolClassCourses'  is null.");
    }

    // GET: SchoolClassCourses/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SchoolClassCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClassCourse schoolClassCourse)
    {
        if (ModelState.IsValid)
        {
            schoolClassCourse.IdGuid = Guid.NewGuid();
            _context.Add(schoolClassCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);
        if (schoolClassCourse == null) return NotFound();
        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClassCourse schoolClassCourse)
    {
        if (id != schoolClassCourse.IdGuid) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassCourseExists(schoolClassCourse.IdGuid))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .FirstOrDefaultAsync(m => m.IdGuid == id);
        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.SchoolClassCourses == null)
            return Problem(
                "Entity set 'DataContextMsSql.SchoolClassCourses'  is null.");
        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);
        if (schoolClassCourse != null)
            _context.SchoolClassCourses.Remove(schoolClassCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SchoolClassCourseExists(Guid id)
    {
        return (_context.SchoolClassCourses?.Any(e => e.IdGuid == id))
            .GetValueOrDefault();
    }
}