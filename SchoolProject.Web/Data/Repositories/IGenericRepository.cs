using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Repositories;

/// <summary>
///     Generic repository interface.
/// </summary>
/// <typeparam name="T"> T is a class </typeparam>
public interface IGenericRepository<T> where T : class, IEntity
{
    /// <summary>
    ///     Get all entities.
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetAll();


    /// <summary>
    ///     Get an entity by it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns> returns an entity or null. </returns>
    Task<T?> GetByIdAsync(int id);


    /// <summary>
    ///     Get an entity by it's idGuid.
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns> returns an entity or null.</returns>
    Task<T?> GetByIdGuidAsync(Guid idGuid);


    /// <summary>
    ///     Create an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>if the entity was successfully created or not.</returns>
    Task<bool> CreateAsync(T entity);


    /// <summary>
    ///     Update an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>if the entity was successfully updated or not.</returns>
    Task<bool> UpdateAsync(T entity);


    /// <summary>
    ///     Delete an entity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>if the entity was successfully deleted or not.</returns>
    Task<bool> DeleteAsync(T entity);


    /// <summary>
    ///     Check if an entity exist, using it's id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> ExistAsync(int id);

    /// <summary>
    ///     Check if an entity exist, using it's idGuid.
    /// </summary>
    /// <param name="idGuid"></param>
    /// <returns>if the entity exist returns true and if not returns false.</returns>
    Task<bool> ExistAsync(Guid idGuid);


    /// <summary>
    ///     Save all changes that have been made.
    /// </summary>
    /// <returns>if the data was successfully save to database returns true and if not returns false.</returns>
    Task<bool> SaveAllAsync();
}