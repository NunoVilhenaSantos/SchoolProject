using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Controller for the Nationalities entity.
/// </summary>
//[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class NationalitiesController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Nationality);
    private const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Name";


    private readonly ICountryRepository _countryRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly INationalityRepository _nationalityRepository;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     Constructor for the NationalitiesController.
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="nationalityRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public NationalitiesController(
        ICountryRepository countryRepository,
        INationalityRepository nationalityRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _countryRepository = countryRepository;
        _nationalityRepository = nationalityRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    // Obtém o controlador atual
    private string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }


    /// <summary>
    ///     NationalityNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult NationalityNotFound => View();


    private List<Nationality> GetNationalitiesWithCountries()
    {
        return _nationalityRepository.GetNationalitiesWithCountries().ToList();
    }


    private List<Nationality> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Nationality> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<Nationality>>(json) ??
                new List<Nationality>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetNationalitiesWithCountries();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Nationality>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Nationalities
    /// <summary>
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Nationality>();
        return View(recordsQuery);
    }

    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Nationality>();
        return View(recordsQuery);
    }


    // GET: Countries
    /// <summary>
    ///     IndexCards1 method for the cards view with pagination mode.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<Nationality>();

        var model = new PaginationViewModel<Nationality>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Nationalities/Details/5
    /// <summary>
    ///     details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(NationalityNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var nationality =
            await _nationalityRepository.GetNationalityAsync(id.Value);

        return nationality == null
            ? new NotFoundViewResult(nameof(NationalityNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(nationality);
    }

    // GET: Nationalities/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }

    // POST: Nationalities/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     create action
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Nationality nationality)
    {
        if (!ModelState.IsValid) return View(nationality);

        await _nationalityRepository.AddNationalityAsync(nationality);

        await _nationalityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Nationalities/Edit/5
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(NationalityNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var nationality = await
            _nationalityRepository.GetNationalityAsync(id.Value);

        return nationality == null
            ? new NotFoundViewResult(nameof(NationalityNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(nationality);
    }

    // POST: Nationalities/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nationality"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Nationality nationality)
    {
        if (id != nationality.Id)
            return new NotFoundViewResult(nameof(NationalityNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(nationality);

        try
        {
            await _nationalityRepository.UpdateNationalityAsync(nationality);
            await _nationalityRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var test = await _nationalityRepository
                .GetNationalityAsync(nationality.Id);

            if (test == null)
                return new NotFoundViewResult(nameof(NationalityNotFound),
                    CurrentClass, id.ToString(), CurrentController,
                    nameof(Index));

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Nationalities/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(NationalityNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var nationality = await _nationalityRepository
            .GetNationalityAsync(id.Value);

        return nationality == null
            ? new NotFoundViewResult(nameof(NationalityNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(nationality);
    }

    // POST: Nationalities/Delete/5
    /// <summary>
    ///     delete action confirmation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nationality =
            await _nationalityRepository.GetNationalityAsync(id);

        if (nationality != null)
            await _nationalityRepository.DeleteNationalityAsync(nationality);

        await _nationalityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }
}