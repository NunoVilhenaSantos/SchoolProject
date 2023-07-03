using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class SchoolClassRepository : GenericRepository<SchoolClass>,
    ISchoolClassRepository
{
    protected SchoolClassRepository(DataContextMssql dataContext) :
        base(dataContext)
    {
    }
}