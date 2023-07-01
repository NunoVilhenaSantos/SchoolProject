using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class GenreMockRepository : IGenericRepository<Genre>
{
    public IQueryable<Genre> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Genre?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Genre entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Genre entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Genre entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        throw new NotImplementedException();
    }
}