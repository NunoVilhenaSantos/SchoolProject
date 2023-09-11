using SchoolProject.Web.Data.Entities.Disciplines;

namespace SchoolProject.Web.Data.Repositories.Mock;

/// <inheritdoc />
public class CourseMockRepository : IGenericRepository<Discipline>
{
    public IQueryable<Discipline> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCount()
    {
        throw new NotImplementedException();
    }

    public async Task<Discipline?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Discipline?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Discipline entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Discipline entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Discipline entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SaveAllAsync()
    {
        throw new NotImplementedException();
    }
}