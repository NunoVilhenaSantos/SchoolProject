using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public class NationalityRepository
    : GenericRepository<Nationality>, INationalityRepository
{
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;
    private readonly DataContextMySql _dataContext;

    // private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;

    private readonly DataContextSqLite _dataContextSqLite;


    /// <inheritdoc />
    public NationalityRepository(
        AuthenticatedUserInApp authenticatedUserInApp,
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;
        // _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _authenticatedUserInApp = authenticatedUserInApp;
    }


    public IQueryable<Nationality> GetNationalitiesWithCountries()
    {
        return _dataContext.Nationalities
            .Include(c => c.Country)
            .ThenInclude(country => country.Cities)
            .OrderBy(c => c.Country.Name)
            .ThenBy(c => c.Name);
    }


    public async Task<Nationality?> GetNationalityAsync(int id)
    {
        return await _dataContext.Nationalities.FindAsync(id);
    }


    public async Task<Nationality?> GetNationalityAsync(Nationality nationality)
    {
        return await _dataContext.Nationalities.FindAsync(nationality.Id);
    }


    public async Task AddNationalityAsync(NationalityViewModel model)
    {
        var country = await GetCountryWithCitiesAsync(model.CountryId);

        if (country == null) return;

        country.Nationality = new()
        {
            Name = model.Name,
            WasDeleted = false,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser(),
            Country = country
            //CountryId = country.Id,
        };

        _dataContext.Countries.Update(country);

        await _dataContext.SaveChangesAsync();
    }

    public async Task AddNationalityAsync(Nationality nationality)
    {
        var country = await GetCountryWithCitiesAsync(nationality);

        if (country == null) return;

        country.Nationality = new()
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


    internal async Task<Country?> GetCountryWithCitiesAsync(int countryId)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c => c.Id == countryId);
    }


    public async Task<Country?> GetCountryWithCitiesAsync(
        Nationality nationality)
    {
        return await _dataContext.Countries
            .Include(c => c.Cities)
            .Include(c => c.Nationality)
            .FirstOrDefaultAsync(c => c.Nationality == nationality);
    }
}