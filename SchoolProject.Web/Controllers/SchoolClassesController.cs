using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassesController : Controller
{
    private readonly DataContextMySql _context;

    public SchoolClassesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: SchoolClasses
    public async Task<IActionResult> Index()
    {
        return _context.SchoolClasses != null
            ? View(model: await _context.SchoolClasses.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.SchoolClasses'  is null.");
    }

    // GET: SchoolClasses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (schoolClass == null) return NotFound();

        return View(model: schoolClass);
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
            include: "Code,Acronym,Name,QnqLevel,EqfLevel,StartDate,EndDate,StartHour,EndHour,Location,Type,Area,PriceForEmployed,PriceForUnemployed,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClass schoolClass)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: schoolClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: schoolClass);
    }

    // GET: SchoolClasses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses.FindAsync(keyValues: id);
        if (schoolClass == null) return NotFound();
        return View(model: schoolClass);
    }

    // POST: SchoolClasses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "Code,Acronym,Name,QnqLevel,EqfLevel,StartDate,EndDate,StartHour,EndHour,Location,Type,Area,PriceForEmployed,PriceForUnemployed,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        SchoolClass schoolClass)
    {
        if (id != schoolClass.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: schoolClass);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassExists(id: schoolClass.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: schoolClass);
    }

    // GET: SchoolClasses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.SchoolClasses == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (schoolClass == null) return NotFound();

        return View(model: schoolClass);
    }

    // POST: SchoolClasses/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.SchoolClasses == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.SchoolClasses'  is null.");
        var schoolClass = await _context.SchoolClasses.FindAsync(keyValues: id);
        if (schoolClass != null) _context.SchoolClasses.Remove(entity: schoolClass);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool SchoolClassExists(int id)
    {
        return (_context.SchoolClasses?.Any(predicate: e => e.Id == id))
            .GetValueOrDefault();
    }
}