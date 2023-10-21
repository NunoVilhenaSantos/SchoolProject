using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Data.Repositories.Countries;

public interface INationalityRepository : IGenericRepository<Nationality>
{
    IOrderedQueryable<Nationality> GetNationalitiesWithCountries();


    IOrderedQueryable<Nationality> GetNationalityAsync(int id);

    IOrderedQueryable<Nationality> GetNationalityAsync(Nationality nationality);


    Task AddNationalityAsync(NationalityViewModel model);

    Task AddNationalityAsync(Nationality nationality);


    Task<int> UpdateNationalityAsync(Nationality nationality);

    Task<int> DeleteNationalityAsync(Nationality nationality);
}