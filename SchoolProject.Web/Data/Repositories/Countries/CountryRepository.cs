using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Countries.ICountryRepository" />
public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

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
    public IEnumerable<Country> GetCountriesWithCities()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .OrderBy(c => c.Name);
    }


    /// <inheritdoc />
    public IEnumerable<Country> GetCountriesWithCitiesEnumerable()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
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

        countriesList.Insert(0, new()
        {
            Text = "(Select a country...)",
            Value = "0"
        });

        return countriesList;
    }

    /// <inheritdoc />
    public IEnumerable<SelectListItem>? GetComboCities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefault(c => c.Id == countryId);


        if (country == null)
            // Retornar uma opção vazia se o país não for encontrado
            return new List<SelectListItem>
            {
                new()
                {
                    Text = "(Select a country...)",
                    Value = "0"
                }
            };

        var citiesList = country.Cities?
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

        // citiesList.Insert(0, new SelectListItem
        // {
        //     Text = "(Select a city...)",
        //     Value = "0"
        // });

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
            {
                new()
                {
                    Text = "(Select a country...)",
                    Value = "0"
                }
            };

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


    /// <summary>
    ///     Get a list of countries and their corresponding nationalities
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SelectListItem> GetComboCountriesAndNationalities()
    {
        // Retrieve countries and their corresponding nationalities
        var countriesWithNationalities =
            _dataContext.Countries
                .Include(c => c.Nationality)
                .ToListAsync().Result.ToList();

        var combinedList =
        (
            from country in countriesWithNationalities.ToList()
            let itemText = $"{country.Name} ({country.Nationality.Name})"
            let itemValue = country.Id.ToString()
            select new SelectListItem
            {
                Text = itemText, Value = itemValue
            }
        ).ToList();

        combinedList.Insert(0, new()
        {
            Text = "(Select a country...)",
            Value = "0"
        });

        return combinedList;
    }


    // ------------------------------ Countries ----------------------------- //

    /// <inheritdoc />
    public async Task<Country?> GetCountryAsync(int cityId)
    {
        var country = await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == cityId));

        return country;
    }


    /// <inheritdoc />
    public async Task<Country?> GetCountryAsync(City city)
    {
        var country = await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c =>
                c.Cities != null &&
                c.Cities.Any(ci => ci.Id == city.Id));

        return country;
    }


    /// <inheritdoc />
    public async Task<Country?> GetCountryWithCitiesAsync(int id)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    /// <inheritdoc />
    public async Task<Country?> GetCountryWithCitiesAsync(Country country)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == country.Id);
    }


    /// <inheritdoc />
    public async Task<Country> GetCountryWithCitiesAsync(City city)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities
                .Where(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();
    }


    // -------------------------------- Cities ------------------------------ //

    /// <inheritdoc />
    public async Task<City?> GetCityAsync(int id)
    {
        return await _dataContext.Cities.FindAsync(id);
    }


    /// <inheritdoc />
    public async Task<City?> GetCityAsync(City city)
    {
        return await _dataContext.Cities.FindAsync(city.Id);
    }


    // ------------------- Cities Add, Update and Delete -------------------- //

    /// <inheritdoc />
    public async Task AddCityAsync(CityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        country.Cities?.Add(new()
        {
            Name = model.Name,
            WasDeleted = false,
            ProfilePhotoId = model.ImageFile == null
                ? Guid.Empty
                : await _storageHelper.UploadFileAsyncToGcp(
                    model.ImageFile, "Cities"),
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            // UpdatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            CountryId = country.Id,
            Country = country
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
    public async Task<Nationality?> GetNationalityAsync(int id)
    {
        return await _dataContext.Nationalities.FindAsync(id);
    }


    /// <inheritdoc />
    public async Task<Nationality?> GetNationalityAsync(Nationality nationality)
    {
        return await _dataContext.Nationalities.FindAsync(nationality.Id);
    }


    // ----------------- Nationality Add, Update and Delete ----------------- //

    /// <inheritdoc />
    public async Task AddNationalityAsync(NationalityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        country.Nationality = new()
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            // UpdatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            Country = country
            //CountryId = country.Id,
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