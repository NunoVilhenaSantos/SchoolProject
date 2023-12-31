﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;

namespace SchoolProject.Web.Controllers.API;

/// <summary>
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class SelectItensController : ControllerBase
{
    private readonly ICountryRepository _countryRepository;
    private readonly DataContextMySql _dataContext;


    /// <summary>
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="dataContext"></param>
    public SelectItensController(
        ICountryRepository countryRepository,
        DataContextMySql dataContext)
    {
        _countryRepository = countryRepository;
        _dataContext = dataContext;
    }

    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Aqui o utilizador obtém a lista de países e a respetiva nacionalidade
    ///     via JSON para o preenchimento do dropdown-list
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    // [Route("api/Helpers/GetCountriesWithNationalitiesJson")]
    [Route("Helpers/GetCountriesWithNationalitiesJson")]
    public Task<JsonResult> GetCountriesWithNationalitiesJson()
    {
        //var countriesWithNationalities =
        //    _countryRepository.GetComboCountriesAndNationalities();

        //return Task.FromResult(new JsonResult(countriesWithNationalities
        //    .OrderBy(c => c.Text).ToJson()));

        var countriesWithNationalities =
            _countryRepository.GetComboCountriesAndNationalities();

        return Task.FromResult(new JsonResult(countriesWithNationalities
            .OrderBy(c => c.Text).ToJson()));
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Aqui o utilizador obtém a lista de cidades de um determinado pais
    /// </summary>
    /// <param name="countryId"></param>
    /// <returns></returns>
    [HttpPost]
    //  [Route("api/Helpers/GetCitiesJson")]
    [Route("GetCitiesJson")]
    public Task<JsonResult> GetCitiesJson(int countryId)
    {
        if (countryId == 0)
            return Task.FromResult(new JsonResult(new List<City>()));

        var cities =
            _countryRepository.GetComboCities(countryId);

        return Task.FromResult(new JsonResult(cities
            .OrderBy(c => c.Text).ToJson()));
    }


    // ---------------------------------------------------------------------- //


    [HttpPost]
    //  [Route("api/Helpers/GetGendersJson")]
    [Route("GetGendersJson")]
    public Task<JsonResult> GetGendersJson()
    {
        var gendersList = _dataContext.Genders
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .OrderBy(c => c.Text)
            .ToList();

        gendersList.Insert(0, new SelectListItem
        {
            Text = "(Select a gender...)",
            Value = "0"
        });

        return Task.FromResult(new JsonResult(gendersList));
    }
}