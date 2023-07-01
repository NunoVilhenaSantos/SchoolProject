using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class EnrollmentMockRepository : IGenericRepository<Enrollment>
{
    public IQueryable<Enrollment> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Enrollment?> GetByIdGuidAsync(Guid idGuid)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Enrollment entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Enrollment entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Enrollment entity)
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