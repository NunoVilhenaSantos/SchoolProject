using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Countries.ICityRepository" />
public class CityRepository : GenericRepository<City>, ICityRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;


    /// <inheritdoc />
    public CityRepository(
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) :
        base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite
        )
    {
        _authenticatedUserInApp = authenticatedUserInApp;

        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    /// <inheritdoc />
    public IOrderedQueryable<City> GetCitiesWithCountriesAsync()
    {
        return _dataContext.Cities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .Include(c => c.Country)
            .ThenInclude(country => country.Nationality)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<City> GetCityAsync(int id)
    {
        // return await _dataContext.Cities.FindAsync(id);

        return _dataContext.Cities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .Include(c => c.Country)
            .ThenInclude(country => country.Nationality)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .Where(c => c.Id == id)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    /// <inheritdoc />
    public IOrderedQueryable<City> GetCityAsync(City city)
    {
        // return await _dataContext.Cities.FindAsync(city.Id);

        return _dataContext.Cities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .Include(c => c.Country)
            .ThenInclude(country => country.Nationality)
            // .Include(c => c.CreatedBy)
            // .Include(c => c.UpdatedBy)
            .Where(c => c == city)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    /// <inheritdoc />
    public async Task AddCityAsync(CityViewModel model)
    {
        var country = GetCountryWithCitiesAsync(model.CountryId)
            .FirstOrDefault();
        if (country == null) return;

        country.Cities?.Add(new City
        {
            Name = model.CityName,
            ProfilePhotoId = default,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            CountryId = country.Id,
            Country = country
        });

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    /// <inheritdoc />
    public async Task AddCityAsync(City city)
    {
        var country = GetCountryWithCitiesAsync(city.Id).FirstOrDefault();

        if (country == null) return;

        country.Cities?.Add(new City
        {
            Name = city.Name,
            ProfilePhotoId = default,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
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


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SelectListItem> GetComboCities()
    {
        var list = _dataContext.Cities
            .OrderBy(a => a.Name)
            .Select(a => new SelectListItem
            {
                Text = $"{a.Name} - {a.Country.Name}",
                Value = a.Id.ToString()
            }).ToList();

        list.Insert(0, new SelectListItem
        {
            Text = "(Select a City...)",
            Value = "0"
        });

        return list;
    }


    // ------------------------ Countries ------------------------------ //


    private IOrderedQueryable<Country> GetCountryWithCitiesAsync(int countryId)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Where(c => c.Id == countryId)
            .OrderBy(c => c.Name);
    }


    private IOrderedQueryable<Country> GetCountryWithCitiesAsync(City city)
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .Where(c => c.Cities != null && c.Cities.Any().Equals(city))
            .OrderBy(c => c.Name);
    }
}