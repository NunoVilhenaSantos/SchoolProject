using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public interface INationalityRepository : IGenericRepository<Nationality>
{
    IQueryable<Country> GetCountriesWithCities();

    IEnumerable<Country> GetCountriesWithCitiesEnumerable();

    IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking();


    IEnumerable<SelectListItem> GetComboCountries();

    IEnumerable<SelectListItem>? GetComboCities(int countryId);


    Task<Country> GetCountryAsync(int modelCityId);

    Task<Country> GetCountryAsync(City city);


    Task<Country?> GetCountryWithCitiesAsync(int id);

    Task<Country?> GetCountryWithCitiesAsync(Country country);

    Task<Country?> GetCountryWithCitiesAsync(City city);


    Task<City?> GetCityAsync(int id);

    Task<City?> GetCityAsync(City city);

    Task<IIncludableQueryable<Country, City>> GetCityWithCountryAsync(int id);

    Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(City city);

    Task<IIncludableQueryable<Country, City>>
        GetCityWithCountryAsync(Country country);


    Task AddCityAsync(CityViewModel model);

    Task<int> UpdateCityAsync(City? city);

    Task<int> DeleteCityAsync(City city);
}