using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Controllers;

public class TeachersController : Controller
{
    private readonly DataContextMySql _context;

    public TeachersController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Teachers
    public async Task<IActionResult> Index()
    {
        return _context.Teachers != null
            ? View(model: await _context.Teachers.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Teachers'  is null.");
    }

    // GET: Teachers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (teacher == null) return NotFound();

        return View(model: teacher);
    }

    // GET: Teachers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Teachers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "FirstName,LastName,Address,PostalCode,MobilePhone,Email,Active,DateOfBirth,IdentificationNumber,IdentificationType,ExpirationDateIdentificationNumber,TaxIdentificationNumber,EnrollDate,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: teacher);
    }

    // GET: Teachers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(keyValues: id);
        if (teacher == null) return NotFound();
        return View(model: teacher);
    }

    // POST: Teachers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "FirstName,LastName,Address,PostalCode,MobilePhone,Email,Active,DateOfBirth,IdentificationNumber,IdentificationType,ExpirationDateIdentificationNumber,TaxIdentificationNumber,EnrollDate,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id: teacher.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: teacher);
    }

    // GET: Teachers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Teachers == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (teacher == null) return NotFound();

        return View(model: teacher);
    }

    // POST: Teachers/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Teachers == null)
            return Problem(detail: "Entity set 'DataContextMySql.Teachers'  is null.");
        var teacher = await _context.Teachers.FindAsync(keyValues: id);
        if (teacher != null) _context.Teachers.Remove(entity: teacher);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool TeacherExists(int id)
    {
        return (_context.Teachers?.Any(predicate: e => e.Id == id)).GetValueOrDefault();
    }
}