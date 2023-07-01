using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>,
    IEnrollmentRepository
{
    protected EnrollmentRepository(DataContextMSSQL dataContext) :
        base(dataContext)
    {
    }
}