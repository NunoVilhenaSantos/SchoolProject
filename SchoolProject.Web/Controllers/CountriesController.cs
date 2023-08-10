using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Controllers;

public class CountriesController : Controller
{
    private readonly DataContextMySql _context;

    public CountriesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Countries
    public async Task<IActionResult> Index()
    {
        return _context.Countries != null
            ? View(model: await _context.Countries.ToListAsync())
            : Problem(detail: "Entity set 'DataContextMySql.Countries'  is null.");
    }

    // GET: Countries/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Countries == null) return NotFound();

        var country = await _context.Countries
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (country == null) return NotFound();

        return View(model: country);
    }

    // GET: Countries/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Countries/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(include: "Name,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Country country)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: country);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: country);
    }

    // GET: Countries/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Countries == null) return NotFound();

        var country = await _context.Countries.FindAsync(keyValues: id);
        if (country == null) return NotFound();
        return View(model: country);
    }

    // POST: Countries/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(include: "Name,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Country country)
    {
        if (id != country.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: country);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id: country.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        return View(model: country);
    }

    // GET: Countries/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Countries == null) return NotFound();

        var country = await _context.Countries
            .FirstOrDefaultAsync(predicate: m => m.Id == id);
        if (country == null) return NotFound();

        return View(model: country);
    }

    // POST: Countries/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Countries == null)
            return Problem(detail: "Entity set 'DataContextMySql.Countries'  is null.");
        var country = await _context.Countries.FindAsync(keyValues: id);
        if (country != null) _context.Countries.Remove(entity: country);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool CountryExists(int id)
    {
        return (_context.Countries?.Any(predicate: e => e.Id == id)).GetValueOrDefault();
    }
}