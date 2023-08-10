using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Controllers;

public class StudentsController : Controller
{
    private readonly DataContextMySql _context;

    public StudentsController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Students
    public async Task<IActionResult> Index()
    {
        return _context.Students != null
            ? View(model: await _context.Students.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Students'  is null.");
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (student == null) return NotFound();

        return View(model: student);
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Students/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "FirstName,LastName,Address,PostalCode,MobilePhone,Email,Active,DateOfBirth,IdentificationNumber,IdentificationType,ExpirationDateIdentificationNumber,TaxIdentificationNumber,EnrollDate,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: student);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: student);
    }

    // GET: Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students.FindAsync(keyValues: id);
        if (student == null) return NotFound();
        return View(model: student);
    }

    // POST: Students/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "FirstName,LastName,Address,PostalCode,MobilePhone,Email,Active,DateOfBirth,IdentificationNumber,IdentificationType,ExpirationDateIdentificationNumber,TaxIdentificationNumber,EnrollDate,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Student student)
    {
        if (id != student.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id: student.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: student);
    }

    // GET: Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (student == null) return NotFound();

        return View(model: student);
    }

    // POST: Students/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Students == null)
            return Problem(detail: "Entity set 'DataContextMySql.Students'  is null.");
        var student = await _context.Students.FindAsync(keyValues: id);
        if (student != null) _context.Students.Remove(entity: student);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool StudentExists(int id)
    {
        return (_context.Students?.Any(predicate: e => e.Id == id)).GetValueOrDefault();
    }
}