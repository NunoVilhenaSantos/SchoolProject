using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Repositories.OtherEntities;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     GendersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class GendersController : Controller
{
    internal const string SessionVarName = "AllGendersList";
    private const string BucketName = "genders";
    private const string SortProperty = "Name";

    private readonly DataContextMySql _context;
    private readonly IGenderRepository _genderRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;


    /// <summary>
    ///     GendersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="genderRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public GendersController(
        DataContextMySql context,
        IGenderRepository genderRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _genderRepository = genderRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    private List<Gender> GendersList()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        return _context.Genders.ToList();
    }


    private List<Gender> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Gender> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Gender>>(json) ??
                           new List<Gender>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GendersList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Gender>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Genders
    /// <summary>
    ///     Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery = SessionData<Gender>();
        return View(recordsQuery);
    }


    // GET: Genders
    /// <summary>
    ///     Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery = SessionData<Gender>();
        return View(recordsQuery);
    }


    // GET: Genders
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<Gender>();

        var model = new PaginationViewModel<Gender>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Genders/Details/5
    /// <summary>
    ///     Details action, to open the view for details.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }


    // GET: Genders/Create
    /// <summary>
    ///     Create action, to open the view for creating.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }


    // POST: Genders/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create action validation and confirmation.
    /// </summary>
    /// <param name="gender"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Gender gender)
    {
        if (!ModelState.IsValid) return View(gender);

        _context.Add(gender);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Genders/Edit/5
    /// <summary>
    ///     Edit action, to open the view for editing.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders.FindAsync(id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // POST: Genders/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit action validation and confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gender"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Gender gender)
    {
        if (id != gender.Id) return NotFound();

        if (!ModelState.IsValid) return View(gender);

        try
        {
            _context.Update(gender);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenderExists(gender.Id)) return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Genders/Delete/5
    /// <summary>
    ///     Delete action, to open the view for confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // POST: Genders/Delete/5
    /// <summary>
    ///     Delete action confirmed.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gender = await _context.Genders.FindAsync(id);

        if (gender != null) _context.Genders.Remove(gender);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool GenderExists(int id)
    {
        return _context.Genders.Any(e => e.Id == id);
    }
}