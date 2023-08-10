using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Repositories.Mock;

/// <inheritdoc />
public class StudentMockRepository : IGenericRepository<Student>
{
    public IQueryable<Student> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Student?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Student entity)
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