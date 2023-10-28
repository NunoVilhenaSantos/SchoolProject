using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Controller for the Nationalities entity.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class NationalitiesController : Controller
{
    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CountriesController.BucketName;
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Nationality.Name);
    internal const string CurrentClass = nameof(Nationality);
    internal const string CurrentAction = nameof(Index);


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(NationalitiesController));


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMailHelper _mailHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;


    // Repositories
    private readonly INationalityRepository _nationalityRepository;
    private readonly ICountryRepository _countryRepository;


    /// <summary>
    ///     Constructor for the NationalitiesController.
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="nationalityRepository"></param>
    /// <param name="storageHelper"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    public NationalitiesController(
        ICountryRepository countryRepository,
        INationalityRepository nationalityRepository,
        IStorageHelper storageHelper,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IUserHelper userHelper,
        IMailHelper mailHelper, IHttpContextAccessor httpContextAccessor)
    {
        _countryRepository = countryRepository;
        _nationalityRepository = nationalityRepository;
        _storageHelper = storageHelper;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _converterHelper = converterHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        _httpContextAccessor = httpContextAccessor;
    }


    // Obtém o controlador atual
    internal string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }


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

            return JsonConvert.DeserializeObject<List<Nationality>>(json) ??
                   new List<Nationality>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = GetNationalitiesWithCountries();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 = PaginationViewModel<Nationality>
            .StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

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
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
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
    public IActionResult IndexCards(
        int pageNumber = 1, int pageSize = 10,
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
    public IActionResult IndexCards1(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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

        var nationality = await _nationalityRepository
            .GetNationalityAsync(id.Value).FirstOrDefaultAsync();

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
        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = nationality.Country.ProfilePhotoId;

        if (nationality.Country.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    nationality.Country.ImageFile, BucketName);

        nationality.Country.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var createdBy = await _authenticatedUserInApp.GetAuthenticatedUser();

        var nationality1 = new Nationality
        {
            Name = nationality.Name,
            Country = null,
            CreatedBy = createdBy,
        };

        var country1 = new Country
        {
            Name = nationality.Country.Name,
            Nationality = nationality1,
            ProfilePhotoId = nationality.Country.ProfilePhotoId,
            CreatedBy = createdBy,
        };

        nationality1.Country = country1;


        await _countryRepository.CreateAsync(country1);

        await _countryRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));


        //**********************
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

        var nationality = await _nationalityRepository
            .GetNationalityAsync(id.Value).FirstOrDefaultAsync();

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
            return new NotFoundViewResult(
                nameof(NationalityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        var nationality1 = await _nationalityRepository
            .GetNationalityAsync(id).FirstOrDefaultAsync();


        if (nationality1 == null) return View(nationality);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = nationality.Country.ProfilePhotoId;

        if (nationality.Country.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    nationality.Country.ImageFile, BucketName);

        nationality.Country.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var updatedBy = await _authenticatedUserInApp.GetAuthenticatedUser();


        nationality1.Name = nationality.Name;
        nationality1.Country.Name = nationality.Country.Name;
        nationality1.Country.ProfilePhotoId = profilePhotoId;
        nationality1.UpdatedBy = updatedBy;


        try
        {
            await _nationalityRepository.UpdateAsync(nationality1);

            await _nationalityRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _nationalityRepository.ExistAsync(nationality.Id))
                return new NotFoundViewResult(
                    nameof(NationalityNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        //*****************
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
            .GetNationalityAsync(id.Value).FirstOrDefaultAsync();

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
        var nationality = await _nationalityRepository.GetByIdAsync(id)
            .FirstOrDefaultAsync();

        if (nationality == null)
            return new NotFoundViewResult(
                nameof(NationalityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _nationalityRepository.DeleteAsync(nationality);

            await _nationalityRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            // Handle DbUpdateException, specifically for this controller.
            Console.WriteLine(ex.Message);

            // Handle foreign key constraint violation.
            DbErrorViewModel dbErrorViewModel;

            if (ex.InnerException != null &&
                ex.InnerException.Message.Contains("DELETE"))
            {
                dbErrorViewModel = new DbErrorViewModel
                {
                    DbUpdateException = true,
                    ErrorTitle = "Foreign Key Constraint Violation",
                    ErrorMessage =
                        "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                        $"The {nameof(Nationality)} with the ID " +
                        $"{nationality.Id} - {nationality.Name} {nationality.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Nationality),
                    ItemId = nationality.Id.ToString(),
                    ItemGuid = nationality.IdGuid,
                    ItemName = nationality.Name
                };

                // Redirecione para o DatabaseError com os dados apropriados
                return RedirectToAction(
                    "DatabaseError", "Errors", dbErrorViewModel);
            }

            // Handle other DbUpdateExceptions.
            dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Database Error",
                ErrorMessage = "An error occurred while deleting the entity.",
                ItemClass = nameof(Nationality),
                ItemId = nationality.Id.ToString(),
                ItemGuid = nationality.IdGuid,
                ItemName = nationality.Name
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    /// <summary>
    ///     NationalityNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult NationalityNotFound()
    {
        return View();
    }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}