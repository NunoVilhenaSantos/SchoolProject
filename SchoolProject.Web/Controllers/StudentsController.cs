﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Controllers;

public class StudentsController : Controller
{
    private readonly DataContextMssql _context;

    public StudentsController(DataContextMssql context)
    {
        _context = context;
    }

    // GET: Students
    public async Task<IActionResult> Index()
    {
        return _context.Students != null
            ? View(await _context.Students.ToListAsync())
            : Problem("Entity set 'DataContextMSSQL.Students'  is null.");
    }

    // GET: Students/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null) return NotFound();

        return View(student);
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Students/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "FirstName,LastName,Address,PostalCode,City,MobilePhone,Email,Active,Genre,DateOfBirth,IdentificationNumber,ExpirationDateIdentificationNumber,TaxIdentificationNumber,Nationality,Birthplace,CoursesCount,TotalWorkHours,EnrollDate,ProfilePhotoId,Id,WasDeleted")]
        Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    // GET: Students/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students.FindAsync(id);
        if (student == null) return NotFound();
        return View(student);
    }

    // POST: Students/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "FirstName,LastName,Address,PostalCode,City,MobilePhone,Email,Active,Genre,DateOfBirth,IdentificationNumber,ExpirationDateIdentificationNumber,TaxIdentificationNumber,Nationality,Birthplace,CoursesCount,TotalWorkHours,EnrollDate,ProfilePhotoId,Id,WasDeleted")]
        Student student)
    {
        if (id != student.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    // GET: Students/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Students == null) return NotFound();

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null) return NotFound();

        return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Students == null)
            return Problem("Entity set 'DataContextMSSQL.Students'  is null.");
        var student = await _context.Students.FindAsync(id);
        if (student != null) _context.Students.Remove(student);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}