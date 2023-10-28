using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;


namespace SchoolProject.Web.Data.Repositories.Countries;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Countries.INationalityRepository" />
public class NationalityRepository
    : GenericRepository<Nationality>, INationalityRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;

    private readonly DataContextMySql _dataContext;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextSqLite _dataContextSqLite;


    /// <inheritdoc />
    public NationalityRepository(
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite) : base(dataContext,
        dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _authenticatedUserInApp = authenticatedUserInApp;

        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IOrderedQueryable<Nationality> GetNationalitiesWithCountries()
    {
        return _dataContext.Nationalities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IOrderedQueryable<Nationality> GetNationalityAsync(int id)
    {
        return _dataContext.Nationalities
            .Include(n => n.Country)
            .ThenInclude(country => country.Cities)
            .Where(e => e.Id == id)
            .OrderBy(n => n.Country.Name);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    public IOrderedQueryable<Nationality> GetNationalityAsync(
        Nationality nationality)
    {
        return _dataContext.Nationalities
            .Include(n => n.Country)
            .ThenInclude(country => country.Cities)
            .Where(e => e.Id == nationality.Id)
            .OrderBy(n => n.Country.Name);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    public async Task AddNationalityAsync(NationalityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        country.Nationality = new Nationality
        {
            Name = model.NationalityName,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            Country = country
            //CountryId = country.Id,
        };

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    public async Task AddNationalityAsync(Nationality nationality)
    {
        var country = await GetCountryWithCitiesAsync(nationality);

        if (country == null) return;

        country.Nationality = new Nationality
        {
            Name = nationality.Name,
            WasDeleted = nationality.WasDeleted,
            CreatedBy = nationality.CreatedBy,
            Country = country
            //CountryId = country.Id,
        };

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    public async Task<int> UpdateNationalityAsync(Nationality nationality)
    {
        var country = await _dataContext.Nationalities
            .Where(n => n.Id == nationality.Id)
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Nationalities.Update(country);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    public async Task<int> DeleteNationalityAsync(Nationality nationality)
    {
        var country = await _dataContext.Nationalities
            .Where(n => n.Id == nationality.Id)
            .FirstOrDefaultAsync();

        if (country == null) return 0;

        _dataContext.Nationalities.Remove(country);

        await _dataContext.SaveChangesAsync();

        return country.Id;
    }


    private async Task<Country?> GetCountryWithCitiesAsync(int countryId)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c => c.Id == countryId);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    private async Task<Country?> GetCountryWithCitiesAsync(
        Nationality nationality)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c => c.Nationality == nationality);
    }
}