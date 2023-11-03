using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <inheritdoc cref="ICountryRepository" />
public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    // helpers
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

    // datacontext
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <inheritdoc />
    public CountryRepository(
        IUserHelper userHelper, IStorageHelper storageHelper,
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _userHelper = userHelper;
        _storageHelper = storageHelper;

        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _authenticatedUserInApp = authenticatedUserInApp;
    }


    // ---------------- Countries, Cities and Nationalities ----------------- //


    // --------------------- List Queryable or Enumerable ------------------- //


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCitiesAndNationalitiesByIdAsync(int id)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Where(c => c.Id == id)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .OrderBy(c => c.Name);
    }

    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountriesWithCities()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public IEnumerable<Country> GetCountriesWithCitiesEnumerable()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .OrderBy(c => c.Name)
            .AsEnumerable();
    }


    // ------------------------- Combo boxes list  -------------------------- //


    /// <inheritdoc />
    public IEnumerable<SelectListItem> GetComboCountries()
    {
        var countriesList = _dataContext.Countries
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

        countriesList.Insert(0, new SelectListItem
        {
            Text = "(Select a country...)",
            Value = "0"
        });

        return countriesList;
    }


    /// <inheritdoc />
    public IEnumerable<SelectListItem> GetComboCities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefault(c => c.Id == countryId);

        // Retornar uma opção vazia se o país não for encontrado
        if (country == null)
            return new List<SelectListItem>
                {new() {Text = "(Select a Country...)", Value = "0"}};

        var citiesList = country.Cities != null
            ? country.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .OrderBy(c => c.Text)
                .ToList()
            : new List<SelectListItem>
                {new() {Text = "(Select a Country...)", Value = "0"}};


        return citiesList;
    }


    /// <inheritdoc />
    public IEnumerable<SelectListItem> GetComboNationalities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Nationality)
            .FirstOrDefault(c => c.Id == countryId);

        if (country == null)
            // Retornar uma opção vazia se o país não for encontrado
            return new List<SelectListItem>
                {new() {Text = "(Select a country...)", Value = "0"}};

        var nationalityItem = new SelectListItem
        {
            Text = country.Nationality.Name,
            Value = country.Nationality.Id.ToString()
        };

        return new List<SelectListItem>
        {
            nationalityItem
        };
    }


    /// <inheritdoc />
    public IEnumerable<SelectListItem> GetComboCountriesAndNationalities()
    {
        var list = _dataContext.Countries
            .Include(o => o.Nationality)
            .Select(p => new SelectListItem
            {
                Text = $"{p.Name} ({p.Nationality.Name})",
                Value = p.Id.ToString()
            }).ToList();

        list.Insert(0,
            new SelectListItem {Text = "(Select a Country....)", Value = "0"});

        return list;
    }


    // ------------------------------ Countries ----------------------------- //


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountryAsync(int cityId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Where(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == cityId))
            .OrderBy(c => c.Name);

        return country;
    }


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountryAsync(City city)
    {
        var country = _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Where(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == city.Id))
            .OrderBy(c => c.Name);

        return country;
    }


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountryWithCitiesAsync(int id)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Where(c => c.Id == id).OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountryWithCitiesAsync(Country country)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Where(c => c.Id == country.Id)
            .OrderBy(c => c.Name)
            .ThenBy(c => c.NumberOfCities);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Country> GetCountryWithCitiesAsync(City city)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Where(ci => ci.Id == city.Id)
            .OrderBy(c => c.Name);
    }


    // -------------------------------- Cities ------------------------------ //

    /// <inheritdoc />
    public IOrderedQueryable<City> GetCitiesByCountryIdAsync(int id)
    {
        return _dataContext.Cities
            .Include(c => c.Country)
            .Where(ci => ci.Country.Id == id)
            .OrderBy(c => c.Name);
    }

    /// <inheritdoc />
    public IOrderedQueryable<City> GetCityAsync(int id)
    {
        return _dataContext.Cities
            .Include(c => c.Country)
            .Where(ci => ci.Id == id)
            .OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<City> GetCityAsync(City city)
    {
        return _dataContext.Cities
            .Include(c => c.Country)
            .Where(ci => ci.Id == city.Id)
            .OrderBy(c => c.Name);
    }


    // ------------------- Cities Add, Update and Delete -------------------- //

    /// <inheritdoc />
    public async Task AddCityAsync(CityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId)
            .FirstOrDefaultAsync();

        if (country == null) return;

        country.Cities?.Add(new City
        {
            Name = model.CityName,

            ProfilePhotoId = model.ImageFile == null
                ? Guid.Empty
                : await _storageHelper.UploadFileAsyncToGcp(
                    model.ImageFile,
                    CountriesController.BucketName),

            Country = country,
            // CountryId = country.Id,

            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            UpdatedBy = await _authenticatedUserInApp.GetAuthenticatedUser()
        });

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    /// <inheritdoc />
    public async Task<int> UpdateCityAsync(City city)
    {
        var country = await _dataContext.Countries
            .Where(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Cities.Update(city);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }


    /// <inheritdoc />
    public async Task<int> DeleteCityAsync(City city)
    {
        var country = await _dataContext.Countries
            .Where(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Cities.Remove(city);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }


    // ---------------------------- Nationalities --------------------------- //

    /// <inheritdoc />
    public IOrderedQueryable<Nationality> GetNationalityAsync(int id)
    {
        return _dataContext.Nationalities
            .Include(o => o.Country)
            .Where(o => o.Id == id)
            .OrderBy(o => o.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<Nationality> GetNationalityAsync(
        Nationality nationality)
    {
        return _dataContext.Nationalities
            .Include(o => o.Country)
            .Where(o => o.Id == nationality.Id)
            .OrderBy(o => o.Name);
    }


    // ----------------- Nationality Add, Update and Delete ----------------- //


    /// <inheritdoc />
    public async Task AddNationalityAsync(NationalityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId)
            .FirstOrDefaultAsync();

        if (country == null) return;

        country.Nationality = new Nationality
        {
            Name = model.NationalityName,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            UpdatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            Country = country
            // CountryId = country.Id,
        };

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    /// <inheritdoc />
    public async Task<int> UpdateNationalityAsync(Nationality nationality)
    {
        var country = await _dataContext.Nationalities
            .Where(n => n.Id == nationality.Id)
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Nationalities.Update(nationality);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }


    /// <inheritdoc />
    public async Task<int> DeleteNationalityAsync(Nationality nationality)
    {
        var country = await _dataContext.Nationalities
            .Where(n => n.Id == nationality.Id)
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Nationalities.Remove(nationality);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }
}