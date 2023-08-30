using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;


namespace SchoolProject.Web.Controllers;

public class TeachersController : Controller
{
    private readonly DataContextMySql _context;

    private const string BucketName = "teachers";


    public TeachersController(DataContextMySql context)
    {
        _context = context;
    }



    private IEnumerable<Teacher> GetTeachersList()
    {
        var teachersList =
            _context.Teachers.ToListAsync();

        return teachersList.Result ?? Enumerable.Empty<Teacher>();
    }




    // GET: Teachers
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    // GET: Teachers
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    // GET: Teachers
    [HttpGet]
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    // GET: Teachers
    [HttpGet]
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }





    // GET: Teachers/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null) return NotFound();

        return View(teacher);
    }



    // GET: Teachers/Create
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(teacher);
    }

    // GET: Teachers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        return View(teacher);
    }

    // POST: Teachers/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();

        if (ModelState.IsValid)
        {
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

        return View(teacher);
    }

    // GET: Teachers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // POST: Teachers/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Teachers == null)
            return Problem("Entity set 'DataContextMySql.Teachers'  is null.");
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null) _context.Teachers.Remove(teacher);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(int id)
    {
        return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}