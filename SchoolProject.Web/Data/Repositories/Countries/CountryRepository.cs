﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;
    private readonly IUserHelper _userHelper;


    /// <inheritdoc />
    protected CountryRepository(
        IUserHelper userHelper,
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext: dataContext, dataContextMySql: dataContextMySql, dataContextMsSql: dataContextMsSql, dataContextSqLite: dataContextSqLite)
    {
        _userHelper = userHelper;
        _dataContext = dataContext;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _authenticatedUserInApp = authenticatedUserInApp;
    }


    public IQueryable<Country> GetCountriesWithCities()
    {
        return _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .OrderBy(keySelector: c => c.Name);
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerable()
    {
        return _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .OrderBy(keySelector: c => c.Name)
            .AsEnumerable();
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking()
    {
        return _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .OrderBy(keySelector: c => c.Name)
            .AsNoTracking().AsEnumerable();
    }


    public IEnumerable<SelectListItem> GetComboCountries()
    {
        var countriesList = _dataContext.Countries
            .Select(selector: c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(keySelector: c => c.Text)
            .ToList();

        countriesList.Insert(index: 0, item: new SelectListItem
        {
            Text = "(Select a country...)",
            Value = "0"
        });

        return countriesList;
    }

    public IEnumerable<SelectListItem>? GetComboCities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .FirstOrDefault(predicate: c => c.Id == countryId);

        if (country == null) return null;

        var citiesList = country.Cities
            .Select(selector: c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(keySelector: c => c.Text)
            .ToList();

        citiesList.Insert(index: 0, item: new SelectListItem
        {
            Text = "(Select a city...)",
            Value = "0"
        });

        return citiesList;
    }


    public async Task<Country> GetCountryAsync(int cityId)
    {
        return await _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .FirstOrDefault(ci => ci.Id == cityId))
            .FirstOrDefaultAsync();
    }


    public async Task<Country> GetCountryAsync(City city)
    {
        return await _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .FirstOrDefault(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();
    }


    public async Task<Country?> GetCountryWithCitiesAsync(int id)
    {
        return await _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .FirstOrDefaultAsync(predicate: c => c.Id == id);
    }

    public async Task<Country?> GetCountryWithCitiesAsync(Country country)
    {
        return await _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities)
            .FirstOrDefaultAsync(predicate: c => c.Id == country.Id);
    }

    public async Task<Country?> GetCountryWithCitiesAsync(City city)
    {
        return await _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .Where(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();
    }

    public async Task<City?> GetCityAsync(int id)
    {
        return await _dataContext.Cities.FindAsync(keyValues: id);
    }

    public async Task<City?> GetCityAsync(City city)
    {
        return await _dataContext.Cities.FindAsync(keyValues: city.Id);
    }


    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(int id)
    {
        return Task.FromResult(result: _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .FirstOrDefault(ci => ci.Id == id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(City city)
    {
        return Task.FromResult(result: _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .FirstOrDefault(ci => ci.Id == city.Id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(Country country)
    {
        return Task.FromResult(result: _dataContext.Countries
            .Include(navigationPropertyPath: c => c.Cities
                .FirstOrDefault(ci => ci.Id == country.Id)));
    }


    public async Task AddCityAsync(CityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(id: model.CountryId);

        if (country == null) return;

        var city = new City
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
        };


        // assim funciona
        //
        // country.Cities.Add(city);
        country.Cities.Add(item: new City
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
        });

        _dataContext.Countries.Update(entity: country);

        await _dataContext.SaveChangesAsync();
    }


    public async Task<int> UpdateCityAsync(City? city)
    {
        var country = await _dataContext.Countries
            .Where(predicate: c => c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Cities.Update(entity: city);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }

    public async Task<int> DeleteCityAsync(City city)
    {
        var country = await _dataContextMsSql.Countries
            .Where(predicate: c => c.Cities.Any(ci => ci.Id == city.Id))
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContextMsSql.Cities.Remove(entity: city);

        await _dataContextMsSql.SaveChangesAsync();

        return country.Id;
    }
}