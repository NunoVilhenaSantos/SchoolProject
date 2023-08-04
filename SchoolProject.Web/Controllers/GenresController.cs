using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Controllers;

public class GenresController : Controller
{
    private readonly DataContextMsSql _context;

    public GenresController(DataContextMsSql context)
    {
        _context = context;
    }

    // GET: Genres
    public async Task<IActionResult> Index()
    {
        return _context.Genres != null
            ? View(await _context.Genres.ToListAsync())
            : Problem("Entity set 'DataContextMsSql.Genres'  is null.");
    }

    // GET: Genres/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Genres == null) return NotFound();

        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // GET: Genres/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Genres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Genre genre)
    {
        if (ModelState.IsValid)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Genres/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Genres == null) return NotFound();

        var genre = await _context.Genres.FindAsync(id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    // POST: Genres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Name,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Genre genre)
    {
        if (id != genre.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Genres/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Genres == null) return NotFound();

        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // POST: Genres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Genres == null)
            return Problem("Entity set 'DataContextMsSql.Genres'  is null.");
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null) _context.Genres.Remove(genre);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(int id)
    {
        return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}