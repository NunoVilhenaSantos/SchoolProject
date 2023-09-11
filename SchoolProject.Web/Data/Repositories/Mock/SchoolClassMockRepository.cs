using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Repositories.Mock;

/// <inheritdoc />
public class SchoolClassMockRepository : IGenericRepository<Course>
{
    public IQueryable<Course> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetCount()
    {
        throw new NotImplementedException();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Course?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Course entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Course entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Course entity)
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