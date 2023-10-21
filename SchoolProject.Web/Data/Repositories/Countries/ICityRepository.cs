using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <summary>
///     City repository
/// </summary>
public interface ICityRepository : IGenericRepository<City>
{
    /// <summary>
    ///     Get all countries with cities.
    /// </summary>
    /// <returns>A queryable list of countries and there cities</returns>
    IOrderedQueryable<City> GetCitiesWithCountriesAsync();


    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<City> GetCityAsync(int id);

    /// <summary>
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    IOrderedQueryable<City> GetCityAsync(City city);


    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddCityAsync(CityViewModel model);

    /// <summary>
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    Task AddCityAsync(City city);


    /// <summary>
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    Task<int> UpdateCityAsync(City city);

    /// <summary>
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    Task<int> DeleteCityAsync(City city);
}