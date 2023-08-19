﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public interface ICountryRepository : IGenericRepository<Country>
{
    IQueryable<Country> GetCountriesWithCities();

    IEnumerable<Country> GetCountriesWithCitiesEnumerable();

    IEnumerable<Country> GetCountriesWithCitiesEnumerableNoTracking();

    IQueryable<Country> GetCountriesWithNationalities();

    IEnumerable<Country> GetCountriesWithNationalitiesEnumerable();


    IEnumerable<SelectListItem> GetComboCountries();

    IEnumerable<SelectListItem> GetCombinedComboCountriesAndNationalities();

    IEnumerable<SelectListItem>? GetComboCities(int countryId);

    IEnumerable<SelectListItem>? GetComboNationalities(int countryId);

    IEnumerable<SelectListItem>? GetComboNationalitiesAsync(int countryId);


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