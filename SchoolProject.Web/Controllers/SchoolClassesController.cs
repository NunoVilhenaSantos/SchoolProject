using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassesController : Controller
{
    private readonly DataContextMsSql _context;

    public SchoolClassesController(DataContextMsSql context)
    {
        _context = context;
    }

    // GET: SchoolClasses
    public async Task<IActionResult> Index()
    {
        return _context.SchoolClasses != null
            ? View(await _context.SchoolClasses.ToListAsync())
            : Problem("Entity set 'DataContextMsSql.SchoolClasses' is null.");
    }

    // GET: SchoolClasses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolClass == null) return NotFound();

        return View(schoolClass);
    }

    // GET: SchoolClasses/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SchoolClasses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "ClassAcronym,ClassName,StartDate,EndDate,StartHour,EndHour,Location,Type,Area,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClass schoolClass)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolClass);
    }

    // GET: SchoolClasses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses.FindAsync(id);
        if (schoolClass == null) return NotFound();
        return View(schoolClass);
    }

    // POST: SchoolClasses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "ClassAcronym,ClassName,StartDate,EndDate,StartHour,EndHour,Location,Type,Area,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClass schoolClass)
    {
        if (id != schoolClass.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClass);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassExists(schoolClass.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(schoolClass);
    }

    // GET: SchoolClasses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolClass == null) return NotFound();

        return View(schoolClass);
    }

    // POST: SchoolClasses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.SchoolClasses == null)
            return Problem(
                "Entity set 'DataContextMsSql.SchoolClasses'  is null.");
        var schoolClass = await _context.SchoolClasses.FindAsync(id);
        if (schoolClass != null) _context.SchoolClasses.Remove(schoolClass);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SchoolClassExists(int id)
    {
        return (_context.SchoolClasses?.Any(e => e.Id == id))
            .GetValueOrDefault();
    }
}