using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesController : Controller
{
    private readonly DataContextMySql _context;

    private const string BucketName = "courses";


    public CoursesController(DataContextMySql context)
    {
        _context = context;
    }


    // Allow unrestricted access to the Index action
    [AllowAnonymous]
    // GET: Courses
    public async Task<IActionResult> Index()
    {
        return View(await _context.Courses.ToListAsync());
    }


    // Allow unrestricted access to the Index action
    [AllowAnonymous]
    // GET: Courses
    public async Task<IActionResult> IndexCards()
    {
        return View(await _context.Courses.ToListAsync());
    }


    // GET: Courses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (course == null) return NotFound();

        return View(course);
    }

    // GET: Courses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Courses/Create
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid) return View(course);

        _context.Add(course);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Courses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses.FindAsync(id);

        if (course == null) return NotFound();

        return View(course);
    }

    // POST: Courses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id) return NotFound();

        if (!ModelState.IsValid) return View(course);

        try
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(course.Id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Courses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null) return NotFound();

        return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course != null) _context.Courses.Remove(course);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool CourseExists(int id)
    {
        return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}