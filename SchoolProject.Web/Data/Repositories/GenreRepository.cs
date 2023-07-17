using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.ExtraTables;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class GenreRepository : GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(DataContextMsSql dataContext) : base(dataContext)
    {
    }
}