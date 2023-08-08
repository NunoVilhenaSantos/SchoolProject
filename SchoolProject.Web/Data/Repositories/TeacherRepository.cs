using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    protected TeacherRepository(DataContextMsSql dataContext) :
        base(dataContext)
    {
    }
}