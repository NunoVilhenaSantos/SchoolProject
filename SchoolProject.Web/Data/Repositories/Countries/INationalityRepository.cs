using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

/// <summary>
///
/// </summary>
public interface INationalityRepository : IGenericRepository<Nationality>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<Nationality> GetNationalitiesWithCountries();


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IOrderedQueryable<Nationality> GetNationalityAsync(int id);


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    IOrderedQueryable<Nationality> GetNationalityAsync(Nationality nationality);


    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task AddNationalityAsync(NationalityViewModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    Task AddNationalityAsync(Nationality nationality);


    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    Task<int> UpdateNationalityAsync(Nationality nationality);

    /// <summary>
    ///
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
    Task<int> DeleteNationalityAsync(Nationality nationality);
}