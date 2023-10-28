using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Countries;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     countries controller, only the admins, superusers and the functionaries
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CountriesController : Controller
{
    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Country.Name);
    internal const string CurrentClass = nameof(Country);
    internal const string CurrentAction = nameof(Index);

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CountriesController));


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMailHelper _mailHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;


    //  repositories
    private readonly INationalityRepository _nationalityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly ICityRepository _cityRepository;


    /// <summary>
    ///     constructor
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="nationalityRepository"></param>
    /// <param name="cityRepository"></param>
    /// <param name="userHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="converterHelper"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="mailHelper"></param>
    public CountriesController(
        ICountryRepository countryRepository,
        IWebHostEnvironment hostingEnvironment,
        INationalityRepository nationalityRepository,
        ICityRepository cityRepository, IUserHelper userHelper,
        IStorageHelper storageHelper, IConverterHelper converterHelper,
        AuthenticatedUserInApp authenticatedUserInApp, IMailHelper mailHelper)
    {
        _countryRepository = countryRepository;
        _hostingEnvironment = hostingEnvironment;
        _converterHelper = converterHelper;
        _authenticatedUserInApp = authenticatedUserInApp;
        _mailHelper = mailHelper;
        _nationalityRepository = nationalityRepository;
        _cityRepository = cityRepository;
        _userHelper = userHelper;
        _storageHelper = storageHelper;
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


    // ------------------------------- ------ ------------------------------- //


    /// <summary>
    ///     Country Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CountryNotFound()
    {
        return View();
    }


    /// <summary>
    ///     City Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CityNotFound()
    {
        return View();
    }


    // ------------------------------ --------- ----------------------------- //
    // ------------------------------ Countries ----------------------------- //
    // ------------------------------ --------- ----------------------------- //


    private List<Country> CountriesWithCities()
    {
        return _countryRepository.GetCountriesWithCities().ToList();
    }


    private List<Country> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Country> recordsQuery;


        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            return JsonConvert.DeserializeObject<List<Country>>(json) ??
                   new List<Country>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = CountriesWithCities();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 =
            PaginationViewModel<Country>.StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

        return recordsQuery;
    }


    // GET: Countries
    /// <summary>
    ///     index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Country>();

        return View(recordsQuery);
    }


    // GET: Countries
    /// <summary>
    ///     index action with cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Country>();

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

        var recordsQuery = SessionData<Country>();

        var model = new PaginationViewModel<Country>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Countries/Details/5
    /// <summary>
    ///     details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository
            .GetCountryWithCitiesAsync(id.Value)
            .FirstOrDefaultAsync();

        return country == null
            ? new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards1))
            : View(country);
    }


    // GET: Countries/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Countries/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     create action
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Country country)
    {
        // if (!ModelState.IsValid) return View(customer);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = country.ProfilePhotoId;

        if (country.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    country.ImageFile, BucketName);

        country.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***

        var nationality1 = new Nationality
        {
            Name = country.Nationality.Name,
            Country = null,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
        };


        var country1 = new Country
        {
            Name = country.Name,
            Nationality = nationality1,
            ProfilePhotoId = country.ProfilePhotoId,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
        };

        nationality1.Country = country1;


        //var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

        // Confirma o email para este AppUser
        //await _userHelper.ConfirmEmailAsync(user, token);

        await _countryRepository.CreateAsync(country1);

        await _countryRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Edit/5
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository
            .GetCountryWithCitiesAsync(id.Value)
            .FirstOrDefaultAsync();

        if (country == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        return View(country);
    }


    // POST: Countries/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Country country)
    {
        if (id != country.Id)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // if (!ModelState.IsValid) return View(country);

        var country1 = await _countryRepository
            .GetCountryWithCitiesAsync(id)
            .FirstOrDefaultAsync();

        if (country1 == null) return View(country);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = country.ProfilePhotoId;

        if (country.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    country.ImageFile, BucketName);

        country.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        country1.Name = country.Name;
        country1.Nationality.Name = country.Nationality.Name;
        country1.ProfilePhotoId = profilePhotoId;
        country1.UpdatedBy =
            await _authenticatedUserInApp.GetAuthenticatedUser();

        try
        {
            await _countryRepository.UpdateAsync(country1);

            await _countryRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _countryRepository.ExistAsync(country.Id))
                return new NotFoundViewResult(
                    nameof(CountryNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }
    }


    // GET: Countries/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository
            .GetCountryWithCitiesAsync(id.Value)
            .FirstOrDefaultAsync();

        return country == null
            ? new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(country);
    }


    // POST: Countries/Delete/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id).FirstOrDefaultAsync();

        if (country == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _countryRepository.DeleteAsync(country);

            await _countryRepository.SaveAllAsync();

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
                        $"The {nameof(Country)} with the ID " +
                        $"{country.Id} - {country.Name} " +
                        // $"{country.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Country), ItemId = country.Id.ToString(),
                    ItemGuid = Guid.Empty, ItemName = country.Name
                };

                // Redirecione para o DatabaseError com os dados apropriados
                return RedirectToAction(
                    "DatabaseError", "Errors", dbErrorViewModel);
            }

            // Handle other DbUpdateExceptions.
            dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true, ErrorTitle = "Database Error",
                ErrorMessage = "An error occurred while deleting the entity.",
                ItemClass = nameof(Country), ItemId = country.Id.ToString(),
                ItemGuid = Guid.Empty, ItemName = country.Name
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    // ------------------------------- ------ ------------------------------- //
    // ------------------------------- Cities ------------------------------- //
    // ------------------------------- ------ ------------------------------- //


    // GET: Countries/AddCity/5
    /// <summary>
    ///     add city action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="countryId"></param>
    /// <param name="countryName"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> AddCity(
        int? id, int countryId, string countryName, int method)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository.GetByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        if (country == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        CityViewModel model;
        switch (method)
        {
            case 0 or 1:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CountryName = country.Name,
                    CityId = 0,
                    CityName = string.Empty
                };
                break;

            case 2:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CountryName = country.Name,
                    CityId = 0,
                    CityName = string.Empty
                };
                break;

            default:
                // algo deu errado
                return new NotFoundViewResult(
                    nameof(CountryNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));
        }


        return View(model);
    }


    // POST: Countries/AddCity
    /// <summary>
    ///     add city action
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddCity(CityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _countryRepository.AddCityAsync(model);

        return RedirectToAction(
            nameof(Details), new {id = model.CountryId});
    }


    // GET: Countries/DeleteCity/5
    /// <summary>
    ///     delete city action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> DeleteCity(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _cityRepository.GetCityAsync(id.Value)
            .FirstOrDefaultAsync();

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var countryId = await _countryRepository.DeleteCityAsync(city);

        return RedirectToAction(nameof(Details), new {id = countryId});
    }


    // GET: Countries/EditCity/5
    /// <summary>
    ///     edit city action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="countryId"></param>
    /// <param name="countryName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> EditCity(
        int? id, int countryId, string countryName)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _cityRepository.GetCityAsync(id.Value)
            .FirstOrDefaultAsync();

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        ViewData["CountryId"] = countryId;
        ViewData["CountryName"] = countryName;

        return View(city);
    }


    // POST: Countries/EditCity/5
    /// <summary>
    ///     edit city action
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> EditCity(int id, City city)
    {
        if (id != city.Id)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // if (!ModelState.IsValid) return View(country);

        var city1 =
            await _cityRepository.GetByIdAsync(id).FirstOrDefaultAsync();

        if (city1 == null) return View(city);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = city.ProfilePhotoId;

        if (city.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    city.ImageFile, BucketName);

        city.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        city1.Name = city.Name;
        city1.ProfilePhotoId = city.ProfilePhotoId;

        try
        {
            await _cityRepository.UpdateAsync(city1);

            await _cityRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _cityRepository.ExistAsync(city.Id))
                return new NotFoundViewResult(
                    nameof(CityNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }
    }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}