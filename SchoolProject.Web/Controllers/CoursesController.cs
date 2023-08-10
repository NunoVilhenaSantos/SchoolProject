using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin, Funcionário")]
public class CoursesController : Controller
{
    private readonly DataContextMySql _context;

    public CoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Courses
    public async Task<IActionResult> Index()
    {
        return _context.Courses != null
            ? View(model: await _context.Courses.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Courses'  is null.");
    }

    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Courses == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (course == null) return NotFound();

        return View(model: course);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Courses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "Code,Name,Description,Hours,CreditPoints,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Course course)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: course);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: course);
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Courses == null) return NotFound();

        var course = await _context.Courses.FindAsync(keyValues: id);
        if (course == null) return NotFound();

        return View(model: course);
    }

    // POST: Courses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "Code,Name,Description,Hours,CreditPoints,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Course course)
    {
        if (id != course.Id) return NotFound();

        if (!ModelState.IsValid) return View(model: course);

        try
        {
            _context.Update(entity: course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(id: course.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(actionName: nameof(Index));
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Courses == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (course == null) return NotFound();

        return View(model: course);
    }

    // POST: Courses/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Courses == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.Courses'  is null.");

        var course = await _context.Courses.FindAsync(keyValues: id);
        if (course != null) _context.Courses.Remove(entity: course);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool CourseExists(int id)
    {
        return (_context.Courses?.Any(predicate: e => e.Id == id)).GetValueOrDefault();
    }
}