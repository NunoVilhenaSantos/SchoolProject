﻿using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Teachers;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Teachers.ITeacherDisciplineRepository" />
public class TeacherDisciplineRepository
    : GenericRepository<TeacherDiscipline>, ITeacherDisciplineRepository
{
    // authenticated user
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // data context
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    // helpers
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <inheritdoc />
    public TeacherDisciplineRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite,
        AuthenticatedUserInApp authenticatedUserInApp,
        IStorageHelper storageHelper, IUserHelper userHelper) : base(
        dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
        _authenticatedUserInApp = authenticatedUserInApp;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
    }


    /// <inheritdoc />
    public Task DeleteRangeAsync(List<TeacherDiscipline> teacherDisciplines)
    {
        _dataContext.TeacherDisciplines.RemoveRange(teacherDisciplines);

        return _dataContext.SaveChangesAsync();
    }


    /// <inheritdoc />
    public IOrderedQueryable<TeacherDiscipline> GetTeacherDisciplines()
    {
        return _dataContext.TeacherDisciplines
            // .Include(scc => scc.Teacher)
            // .Include(scc => scc.Discipline)
            // --------------------- Discipline section --------------------- //
            .Include(td => td.Discipline)

            // --------------------- Teacher section -------------------- //
            .Include(td => td.Teacher)
            // .ThenInclude(t => t.City)
            // .ThenInclude(c => c.Country)
            // .ThenInclude(c => c.CreatedBy)
            // .Include(td => td.Teacher)
            // .ThenInclude(t => t.CountryOfNationality)
            // .ThenInclude(c => c.Nationality)
            // .ThenInclude(c => c.CreatedBy)
            // .Include(td => td.Teacher)
            // .ThenInclude(t => t.Birthplace)
            // .ThenInclude(c => c.Nationality)
            // .ThenInclude(c => c.CreatedBy)
            // .Include(td => td.Teacher)
            // .ThenInclude(t => t.Gender)
            // .ThenInclude(g => g.CreatedBy)
            // .Include(td => td.Teacher)
            // .ThenInclude(t => t.AppUser)
            // .ThenInclude(u => u.CreatedBy)

            // --------------------- Others section --------------------- //
            // .Include(td => td.CreatedBy)
            // .Include(td => td.UpdatedBy)
            // .Where(i => i.Id == id)
            .OrderBy(o => o.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<TeacherDiscipline>
        GetTeacherDisciplinesById(int id)
    {
        return _dataContext.TeacherDisciplines
            .Include(td => td.Discipline)
            .Include(td => td.Teacher)
            .Include(td => td.CreatedBy)
            .Include(td => td.UpdatedBy)
            .Where(i => i.Id == id)
            .OrderBy(o => o.Teacher.Id);
    }


    /// <inheritdoc />
    public IOrderedQueryable<TeacherDiscipline>
        GetTeacherDisciplinesByIdGuid(Guid idGuid)
    {
        return _dataContext.TeacherDisciplines
            .Include(td => td.Discipline)
            .Include(td => td.Teacher)
            .Include(td => td.CreatedBy)
            .Include(td => td.UpdatedBy)
            .Where(i => i.IdGuid == idGuid)
            .OrderBy(o => o.Teacher.Id);
    }
}