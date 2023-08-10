using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Controllers;

public class NationalitiesController : Controller
{
    private readonly DataContextMySql _context;

    public NationalitiesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Nationalities
    public async Task<IActionResult> Index()
    {
        return _context.Nationalities != null
            ? View(model: await _context.Nationalities.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Nationalities'  is null.");
    }

    // GET: Nationalities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Nationalities == null) return NotFound();

        var nationality = await _context.Nationalities
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (nationality == null) return NotFound();

        return View(model: nationality);
    }

    // GET: Nationalities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Nationalities/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(include: "Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Nationality nationality)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: nationality);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: nationality);
    }

    // GET: Nationalities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Nationalities == null) return NotFound();

        var nationality = await _context.Nationalities.FindAsync(keyValues: id);
        if (nationality == null) return NotFound();
        return View(model: nationality);
    }

    // POST: Nationalities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(include: "Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Nationality nationality)
    {
        if (id != nationality.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: nationality);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationalityExists(id: nationality.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: nationality);
    }

    // GET: Nationalities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Nationalities == null) return NotFound();

        var nationality = await _context.Nationalities
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (nationality == null) return NotFound();

        return View(model: nationality);
    }

    // POST: Nationalities/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Nationalities == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.Nationalities'  is null.");
        var nationality = await _context.Nationalities.FindAsync(keyValues: id);
        if (nationality != null) _context.Nationalities.Remove(entity: nationality);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool NationalityExists(int id)
    {
        return (_context.Nationalities?.Any(predicate: e => e.Id == id))
            .GetValueOrDefault();
    }
}