using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories.Mock;

public class MockRepository : IGenericRepository<Owner>
{
    public IQueryable<Owner> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Owner?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateAsync(Owner entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(Owner entity)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Owner entity)
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

    public Owner? GetOwner(int id)
    {
        throw new NotImplementedException();
    }


    public void AddOwner(Owner? owner)
    {
        throw new NotImplementedException();
    }


    public void UpdateOwner(Owner? owner)
    {
        throw new NotImplementedException();
    }


    public void RemoveOwner(Owner? owner)
    {
        throw new NotImplementedException();
    }


    public Task<bool> SaveOwnersAsync()
    {
        throw new NotImplementedException();
    }


    public bool OwnerExists(int id)
    {
        throw new NotImplementedException();
    }

    public IOrderedQueryable<Owner?> GetOwners()
    {
        throw new NotImplementedException();
    }
}