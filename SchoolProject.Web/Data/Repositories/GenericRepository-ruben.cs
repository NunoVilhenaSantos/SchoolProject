using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Repositories.Interfaces;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Repositories
{
    public class GenericRepositoryRuben<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContextMsSql _context;

        public GenericRepositoryRuben(DataContextMsSql context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdGuidAsync(Guid idGuid)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IGenericRepository<T>.CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IGenericRepository<T>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IGenericRepository<T>.DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }
    }
}
