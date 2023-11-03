using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Repositories;

/// <inheritdoc />
public class GenericRepository<T> : IGenericRepository<T>
    where T : class, IEntity
{
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;


    /// <summary>
    ///     Constructor of the generic repository.
    /// </summary>
    /// <param name="dataContext"></param>
    /// <param name="dataContextMySql"></param>
    /// <param name="dataContextMsSql"></param>
    /// <param name="dataContextSqLite"></param>
    protected GenericRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    )
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    /// <inheritdoc />
    public IQueryable<T> GetAll()
    {
        // return _dataContext.Set<T>().AsNoTracking();

        return _dataContext.Set<T>().AsQueryable().AsNoTracking();

        // return _dataContext.Set<T>().Select(e => new ObjectViewModel { Property = e.Property }).AsQueryable();

        // usa
        // _context.Set<T>().Select(e => new ObjectViewModel { Property = e.Property } );
        // em vez de
        // _context.Set<T>().Find(); 
        // ou
        // _context.Set<T>().AsNoTracking().First();
    }


    /// <inheritdoc />
    public async Task<int> GetCount()
    {
        return await _dataContext.Set<T>().CountAsync();
        // return await _dataContext.Set<T>().TryGetNonEnumeratedCount();
    }


    /// <inheritdoc />
    public IQueryable<T> GetByIdAsync(int id)
    {
        // return await _dataContext.Set<T>().FindAsync(id).AsTask();
        return _dataContext.Set<T>().AsQueryable()
            .Where(e => e.Id == id);
    }


    /// <inheritdoc />
    public IQueryable<T> GetByIdGuidAsync(Guid idGuid)
    {
        return _dataContext.Set<T>().AsQueryable()
            .Where(e => e.IdGuid == idGuid);
    }


    /// <inheritdoc />
    public async Task<bool> CreateAsync(T entity)
    {
        await _dataContext.Set<T>().AddAsync(entity);
        return await SaveAllAsync();
    }


    /// <inheritdoc />
    public async Task<bool> UpdateAsync(T entity)
    {
        _dataContext.Set<T>().Update(entity);
        return await SaveAllAsync();
    }


    /// <inheritdoc />
    public async Task<bool> DeleteAsync(T entity)
    {
        _dataContext.Set<T>().Remove(entity);
        return await SaveAllAsync();
    }


    /// <inheritdoc />
    public async Task<bool> ExistAsync(int id)
    {
        return await _dataContext.Set<T>().AnyAsync(e => e.Id == id);
    }


    /// <inheritdoc />
    public async Task<bool> ExistAsync(Guid idGuid)
    {
        return await _dataContext.Set<T>().AnyAsync(e => e.IdGuid == idGuid);
    }


    /// <inheritdoc />
    public async Task<bool> SaveAllAsync()
    {
        return await _dataContext.SaveChangesAsync() > 0;
    }
}