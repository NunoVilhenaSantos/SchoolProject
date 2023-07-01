using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    protected StudentRepository(DataContextMSSQL dataContext) :
        base(dataContext)
    {
    }
}