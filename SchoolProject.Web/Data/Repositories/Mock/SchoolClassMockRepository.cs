using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class SchoolClassMockRepository : IGenericRepository<SchoolClass>
{
    public IQueryable<SchoolClass> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<SchoolClass?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SchoolClass?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(SchoolClass entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(SchoolClass entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(SchoolClass entity)
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