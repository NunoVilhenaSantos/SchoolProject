using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <summary>
///     ICountryRepository interface.
/// </summary>
public interface ICountryRepository : IGenericRepository<Country>
{
    // ---------------- Countries, Cities and Nationalities ----------------- //


    // --------------------- List Queryable or Enumerable ------------------- //
    /// <summary>
    ///     Get countries with cities.
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountriesWithCities();

    IOrderedQueryable<Country> GetCitiesAndNationalitiesByIdAsync(int id);


    /// <summary>
    ///     Get countries with cities.
    /// </summary>
    /// <returns></returns>
    IEnumerable<Country> GetCountriesWithCitiesEnumerable();


    // ------------------------- Combo boxes list  -------------------------- //
    /// <summary>
    ///     Get combo countries.
    /// </summary>
    /// <returns></returns>
    IEnumerable<SelectListItem> GetComboCountries();

    /// <summary>
    ///     Get combo countries and nationalities.
    /// </summary>
    /// <returns></returns>
    IEnumerable<SelectListItem> GetComboCountriesAndNationalities();

    /// <summary>
    ///     Get combo cities.
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    IEnumerable<SelectListItem>? GetComboCities(int countryId);

    /// <summary>
    ///     Get combo nationalities.
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    IEnumerable<SelectListItem> GetComboNationalities(int countryId);


    // ------------------------------ Countries ----------------------------- //

    /// <summary>
    ///     Get country.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountryAsync(int id);

    /// <summary>
    ///     Get country.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountryAsync(City city);


    /// <summary>
    ///     Get country with cities.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountryWithCitiesAsync(int id);

    /// <summary>
    ///     Get country with cities.
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountryWithCitiesAsync(Country country);

    /// <summary>
    ///     Get country with cities.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    IOrderedQueryable<Country> GetCountryWithCitiesAsync(City city);


    // -------------------------------- Cities ------------------------------ //

    IOrderedQueryable<City> GetCitiesByCountryIdAsync(int id);

    /// <summary>
    ///     Get city.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<City> GetCityAsync(int id);

    /// <summary>
    ///     Get city.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    IOrderedQueryable<City> GetCityAsync(City city);


    // ------------------- Cities Add, Update and Delete -------------------- //

    /// <summary>
    ///     Add city.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddCityAsync(CityViewModel model);

    /// <summary>
    ///     update city.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    Task<int> UpdateCityAsync(City city);


    /// <summary>
    ///     Delete city.
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
    Task<int> DeleteCityAsync(City city);


    // ---------------------------- Nationalities --------------------------- //

    /// <summary>
    ///     get nationality.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Nationality> GetNationalityAsync(int id);

    /// <summary>
    ///     get nationality.
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    IOrderedQueryable<Nationality> GetNationalityAsync(Nationality nationality);


    // ----------------- Nationality Add, Update and Delete ----------------- //

    /// <summary>
    ///     add nationality.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddNationalityAsync(NationalityViewModel model);


    /// <summary>
    ///     update nationality.
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    Task<int> UpdateNationalityAsync(Nationality nationality);


    /// <summary>
    ///     delete nationality.
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    Task<int> DeleteNationalityAsync(Nationality nationality);
}