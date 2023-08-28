using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.GenericRepository<T>" />
public class CityRepository : GenericRepository<City>, ICityRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

    private readonly DataContextSqLite _dataContextSqLite;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextMySql _dataContext;


    /// <inheritdoc />
    public CityRepository(
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _authenticatedUserInApp = authenticatedUserInApp;
    }


    /// <inheritdoc />
    public IQueryable<City> GetCitiesWithCountriesAsync()
    {
        return _dataContext.Cities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    private async Task<Country?> GetCountryWithCitiesAsync(int countryId)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == countryId);
    }


    private async Task<Country?> GetCountryWithCitiesAsync(City city)
    {
        //var country = await _dataContext.Countries
        //    .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
        //    .FirstOrDefaultAsync();

        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();
    }


    public async Task<City?> GetCityAsync(int id)
    {
        return await _dataContext.Cities.FindAsync(id);
    }


    public async Task<City?> GetCityAsync(City city)
    {
        return await _dataContext.Cities.FindAsync(city.Id);
    }


    public async Task AddCityAsync(CityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        country.Cities?.Add(new City
        {
            Name = model.Name,
            ProfilePhotoId = default,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            CountryId = country.Id,
            Country = country,
        });

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }

    public async Task AddCityAsync(City city)
    {
        var country = await GetCountryWithCitiesAsync(city.Id);

        if (country == null) return;

        country.Cities?.Add(new City
        {
            Name = city.Name,
            ProfilePhotoId = default,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            CountryId = country.Id,
            Country = country,
        });

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    public async Task<int> UpdateCityAsync(City city)
    {
        var country = await _dataContext.Countries
            .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Cities.Update(city);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }

    public async Task<int> DeleteCityAsync(City city)
    {
        var country = await _dataContext.Countries
            .Where(c => c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Cities.Remove(city);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }
}