using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class GenreMockRepository : IGenericRepository<Gender>
{
    public IQueryable<Gender> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Gender?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Gender?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Gender entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Gender entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Gender entity)
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