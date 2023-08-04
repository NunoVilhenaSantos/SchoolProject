using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Controllers
{
    public class TeacherCoursesController : Controller
    {
        private readonly DataContextMsSql _context;

        public TeacherCoursesController(DataContextMsSql context)
        {
            _context = context;
        }

        // GET: TeacherCourses
        public async Task<IActionResult> Index()
        {
            return _context.TeacherCourses != null
                ? View(await _context.TeacherCourses.ToListAsync())
                : Problem(
                    "Entity set 'DataContextMsSql.TeacherCourses'  is null.");
        }

        // GET: TeacherCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherCourse = await _context.TeacherCourses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherCourse == null)
            {
                return NotFound();
            }

            return View(teacherCourse);
        }

        // GET: TeacherCourses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeacherCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
            TeacherCourse teacherCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(teacherCourse);
        }

        // GET: TeacherCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherCourse = await _context.TeacherCourses.FindAsync(id);
            if (teacherCourse == null)
            {
                return NotFound();
            }

            return View(teacherCourse);
        }

        // POST: TeacherCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
            TeacherCourse teacherCourse)
        {
            if (id != teacherCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherCourseExists(teacherCourse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(teacherCourse);
        }

        // GET: TeacherCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeacherCourses == null)
            {
                return NotFound();
            }

            var teacherCourse = await _context.TeacherCourses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherCourse == null)
            {
                return NotFound();
            }

            return View(teacherCourse);
        }

        // POST: TeacherCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeacherCourses == null)
            {
                return Problem(
                    "Entity set 'DataContextMsSql.TeacherCourses'  is null.");
            }

            var teacherCourse = await _context.TeacherCourses.FindAsync(id);
            if (teacherCourse != null)
            {
                _context.TeacherCourses.Remove(teacherCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherCourseExists(int id)
        {
            return (_context.TeacherCourses?.Any(e => e.Id == id))
                .GetValueOrDefault();
        }
    }
}