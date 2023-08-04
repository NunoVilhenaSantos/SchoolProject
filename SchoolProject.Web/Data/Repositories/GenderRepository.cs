using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class GenderRepository : GenericRepository<Gender>, IGenreRepository
{
    public GenderRepository(DataContextMsSql dataContext) : base(dataContext)
    {
    }
}