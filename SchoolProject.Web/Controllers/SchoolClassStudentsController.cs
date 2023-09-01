using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassStudentsController : Controller
{
    private readonly DataContextMySql _context;

    public SchoolClassStudentsController(DataContextMySql context)
    {
        _context = context;
    }


    private IEnumerable<SchoolClassStudent> GetSchoolClassesAndStudent()
    {
        var schoolClassesStudentList =
            _context.SchoolClassStudents
                .Include(s => s.SchoolClass)
                .Include(s => s.Student)
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy);


        return schoolClassesStudentList ??
               Enumerable.Empty<SchoolClassStudent>();
    }


    // GET: SchoolClassStudents
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesAndStudent());
    }


    // GET: SchoolClassStudents
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesAndStudent());
    }


    // GET: SchoolClassStudents
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesAndStudent());
    }


    // GET: SchoolClassStudents
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesAndStudent());
    }

    // GET: SchoolClassStudents/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassStudent = await _context.SchoolClassStudents
            .Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassStudent == null) return NotFound();

        return View(schoolClassStudent);
    }

    // GET: SchoolClassStudents/Create
    public IActionResult Create()
    {
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses, "Id", "Acronym");
        ViewData["StudentId"] =
            new SelectList(_context.Students, "Id", "Address");
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: SchoolClassStudents/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        SchoolClassStudent schoolClassStudent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CreatedById"] =
            new SelectList(_context.Users, "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses, "Id",
                "Acronym", schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students, "Id",
                "Address", schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(
                _context.Users, "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }

    // GET: SchoolClassStudents/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassStudent =
            await _context.SchoolClassStudents.FindAsync(id);

        if (schoolClassStudent == null) return NotFound();

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }

    // POST: SchoolClassStudents/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, SchoolClassStudent schoolClassStudent)
    {
        if (id != schoolClassStudent.SchoolClassId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassStudent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassStudentExists(schoolClassStudent.SchoolClassId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassStudent.SchoolClassId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                schoolClassStudent.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassStudent.UpdatedById);

        return View(schoolClassStudent);
    }

    // GET: SchoolClassStudents/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var schoolClassStudent = await _context.SchoolClassStudents
            .Include(s => s.CreatedBy)
            .Include(s => s.SchoolClass)
            .Include(s => s.Student)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassStudent == null) return NotFound();

        return View(schoolClassStudent);
    }

    // POST: SchoolClassStudents/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClassStudent =
            await _context.SchoolClassStudents.FindAsync(id);

        if (schoolClassStudent != null)
            _context.SchoolClassStudents.Remove(schoolClassStudent);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool SchoolClassStudentExists(int id)
    {
        return (_context.SchoolClassStudents?
                .Any(e => e.SchoolClassId == id))
            .GetValueOrDefault();
    }
}