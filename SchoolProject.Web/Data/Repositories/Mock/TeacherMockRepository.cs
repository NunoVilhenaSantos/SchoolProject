using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class TeacherMockRepository : IGenericRepository<Teacher>
{
    public IQueryable<Teacher> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Teacher?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Teacher entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Teacher entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Teacher entity)
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