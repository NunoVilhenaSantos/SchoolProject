using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Controllers;

public class CitiesController : Controller
{
    private readonly DataContextMySql _context;

    public CitiesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Cities
    public async Task<IActionResult> Index()
    {
        return _context.Cities != null
            ? View(model: await _context.Cities.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Cities'  is null.");
    }

    // GET: Cities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Cities == null) return NotFound();

        var city = await _context.Cities
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (city == null) return NotFound();

        return View(model: city);
    }

    // GET: Cities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cities/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(include: "Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        City city)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: city);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: city);
    }

    // GET: Cities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Cities == null) return NotFound();

        var city = await _context.Cities.FindAsync(keyValues: id);
        if (city == null) return NotFound();
        return View(model: city);
    }

    // POST: Cities/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(include: "Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        City city)
    {
        if (id != city.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: city);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id: city.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: city);
    }

    // GET: Cities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Cities == null) return NotFound();

        var city = await _context.Cities
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (city == null) return NotFound();

        return View(model: city);
    }

    // POST: Cities/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Cities == null)
            return Problem(detail: "Entity set 'DataContextMySql.Cities'  is null.");
        var city = await _context.Cities.FindAsync(keyValues: id);
        if (city != null) _context.Cities.Remove(entity: city);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool CityExists(int id)
    {
        return (_context.Cities?.Any(predicate: e => e.Id == id)).GetValueOrDefault();
    }
}