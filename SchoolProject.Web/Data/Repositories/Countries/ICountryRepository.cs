using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public interface ICountryRepository : IGenericRepository<Country>
{
    // ---------------- Countries, Cities and Nationalities ----------------- //


    // --------------------- List Queryable or Enumerable ------------------- //
    IQueryable<Country> GetCountriesWithCities();

    IEnumerable<Country> GetCountriesWithCitiesEnumerable();


    // ------------------------- Combo boxes list  -------------------------- //
    IEnumerable<SelectListItem> GetComboCountries();

    IEnumerable<SelectListItem> GetComboCountriesAndNationalities();

    IEnumerable<SelectListItem> GetComboCities(int countryId);

    IEnumerable<SelectListItem> GetComboNationalities(int countryId);


    // ------------------------------ Countries ----------------------------- //

    Task<Country?> GetCountryAsync(int id);

    Task<Country?> GetCountryAsync(City city);


    Task<Country?> GetCountryWithCitiesAsync(int id);

    Task<Country?> GetCountryWithCitiesAsync(Country country);

    Task<Country?> GetCountryWithCitiesAsync(City city);


    // -------------------------------- Cities ------------------------------ //

    Task<City?> GetCityAsync(int id);

    Task<City?> GetCityAsync(City city);


    // ------------------- Cities Add, Update and Delete -------------------- //

    Task AddCityAsync(CityViewModel model);

    Task<int> UpdateCityAsync(City city);

    Task<int> DeleteCityAsync(City city);


    // ---------------------------- Nationalities --------------------------- //

    Task<Nationality?> GetNationalityAsync(int id);

    Task<Nationality?> GetNationalityAsync(Nationality nationality);


    // ----------------- Nationality Add, Update and Delete ----------------- //

    Task AddNationalityAsync(NationalityViewModel model);

    Task<int> UpdateNationalityAsync(Nationality nationality);

    Task<int> DeleteNationalityAsync(Nationality nationality);
}