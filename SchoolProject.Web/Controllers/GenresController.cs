using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Controllers;

public class GenresController : Controller
{
    private readonly DataContextMSSQL _context;

    public GenresController(DataContextMSSQL context)
    {
        _context = context;
    }

    // GET: Genres
    public async Task<IActionResult> Index()
    {
        return _context.Genre != null
            ? View(await _context.Genre.ToListAsync())
            : Problem("Entity set 'DataContextMSSQL.Genre'  is null.");
    }

    // GET: Genres/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Genre == null) return NotFound();

        var genre = await _context.Genre
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
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,WasDeleted,Name")] Genre genre)
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
        if (id == null || _context.Genre == null) return NotFound();

        var genre = await _context.Genre.FindAsync(id);
        if (genre == null) return NotFound();
        return View(genre);
    }

    // POST: Genres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,WasDeleted,Name")] Genre genre)
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
        if (id == null || _context.Genre == null) return NotFound();

        var genre = await _context.Genre
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
        if (_context.Genre == null)
            return Problem("Entity set 'DataContextMSSQL.Genre'  is null.");
        var genre = await _context.Genre.FindAsync(id);
        if (genre != null) _context.Genre.Remove(genre);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(int id)
    {
        return (_context.Genre?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}