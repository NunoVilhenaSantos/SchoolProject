using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Data.Repositories.Interfaces;

namespace SchoolProject.Web.Data.Repositories;

public class LesseeRepository : GenericRepository<Lessee>, ILesseeRepository
{
    public LesseeRepository(DataContext dataContext) : base(dataContext)
    {
    }
}