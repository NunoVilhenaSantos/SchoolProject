using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>,
    IEnrollmentRepository
{
    protected EnrollmentRepository(DataContextMsSql dataContext) :
        base(dataContext)
    {
    }
}