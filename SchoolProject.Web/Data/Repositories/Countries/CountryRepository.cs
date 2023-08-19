using Microsoft.AspNetCore.Mvc.Rendering;
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
    public CountryRepository(
        IUserHelper userHelper,
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
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
            .Include(c => c.Cities)
            .OrderBy(c => c.Name);
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerable()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .OrderBy(c => c.Name)
            .AsEnumerable();
    }

    public IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking()
    {
        return _dataContext.Countries
            .Include(c => c.Cities)
            .OrderBy(c => c.Name)
            .AsNoTracking().AsEnumerable();
    }

    public IQueryable<Country> GetCountriesWithNationalities()
    {
        return _dataContext.Countries
            .Include(c => c.Nationality)
            .OrderBy(c => c.Name);
    }

    public IEnumerable<Country> GetCountriesWithNationalitiesEnumerable()
    {
        return _dataContext.Countries
            .Include(c => c.Nationality)
            .OrderBy(c => c.Name)
            .AsEnumerable();
    }


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

    public IEnumerable<SelectListItem>? GetComboCities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefault(c => c.Id == countryId);

        // if (country == null) return null;

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

        var citiesList = country.Cities
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

        citiesList.Insert(0, new SelectListItem
        {
            Text = "(Select a city...)",
            Value = "0"
        });

        return citiesList;
    }


    public IEnumerable<SelectListItem>? GetComboNationalities(int countryId)
    {
        var country = _dataContext.Countries
            .Include(c => c.Nationality)
            .FirstOrDefault(c => c.Id == countryId);

        // if (country == null) return null;

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

    public IEnumerable<SelectListItem>? GetComboNationalitiesAsync(
        int countryId)
    {
        var nationalities = _dataContext.Countries
            .Include(c => c.Nationality)
            .Where(c => c.Id == countryId)
            .Select(c => c.Nationality)
            .Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString()
            });

        return nationalities;
    }


    public IEnumerable<SelectListItem>
        GetCombinedComboCountriesAndNationalities()
    {
        var combinedList = new List<SelectListItem>();

        // Retrieve countries and their corresponding nationalities
        var countriesWithNationalities = _dataContext.Countries
            .Include(c => c.Nationality).ToListAsync().Result;

        countriesWithNationalities.ToList();

        foreach (var country in countriesWithNationalities)
        {
            var itemText = $"{country.Name} ({country.Nationality.Name})";
            var itemValue =
                country.Id.ToString(); // Use the appropriate ID property

            combinedList.Add(new SelectListItem
                {Text = itemText, Value = itemValue});
        }

        combinedList.Insert(0, new SelectListItem
        {
            Text = "(Select a country...)",
            Value = "0"
        });

        return combinedList;
    }


    public async Task<Country> GetCountryAsync(int cityId)
    {
        var country = await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(
                c => c.Cities.Any(ci => ci.Id == cityId));

        return country;
    }


    public async Task<Country> GetCountryAsync(City city)
    {
        var country = await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(
                c => c.Cities.Any(ci => ci.Id == city.Id));

        return country;
    }


    public async Task<Country?> GetCountryWithCitiesAsync(int id)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Country?> GetCountryWithCitiesAsync(Country country)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .FirstOrDefaultAsync(c => c.Id == country.Id);
    }

    public async Task<Country?> GetCountryWithCitiesAsync(City city)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities
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
        return Task.FromResult(_dataContext.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(City city)
    {
        return Task.FromResult(_dataContext.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == city.Id)));
    }

    public Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(Country country)
    {
        return Task.FromResult(_dataContext.Countries
            .Include(c => c.Cities
                .FirstOrDefault(ci => ci.Id == country.Id)));
    }


    public async Task AddCityAsync(CityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        var city = new City
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser()
        };


        // assim funciona
        //
        // country.Cities.Add(city);
        country.Cities.Add(new City
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser()
        });

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    public async Task<int> UpdateCityAsync(City? city)
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