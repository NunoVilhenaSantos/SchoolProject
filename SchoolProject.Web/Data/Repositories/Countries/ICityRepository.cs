using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public interface ICityRepository : IGenericRepository<City>
{
    /// <summary>
    ///     Get all countries with cities.
    /// </summary>
    /// <returns>A queryable list of countries and there cities</returns>
    IQueryable<City> GetCitiesWithCountriesAsync();


    Task<City?> GetCityAsync(int id);

    Task<City?> GetCityAsync(City city);


    Task AddCityAsync(CityViewModel model);

    Task AddCityAsync(City city);


    Task<int> UpdateCityAsync(City city);

    Task<int> DeleteCityAsync(City city);
}